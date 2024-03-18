// import getEnv from '../../utils/getEnv';
import axiosClient from '../../utils/httpClient/axiosClient';
import { tokenService } from './tokenService';

export type Credencials = {
  id: string;
  name: string;
  email: string;
  roleName: string;
  token: string;
};

export type LoginResponseBody = {
  valid: boolean;
  credentials: Credencials;
};

type LoginBody = { email: string; password: string };
//type LoginResponse = { token: string };

export const authService = {
  async login({ email, password }: LoginBody) {
    return axiosClient
      .post<LoginResponseBody>('/Auth/login', {
        email,
        password,
      })
      .then((response) => {
        console.log('response');
        console.log(response.data);
        if (response.status !== 200)
          throw new Error('Usuário ou senha inválidos');
        const body = response.data;
        if (body.credentials.token) tokenService.save(body.credentials.token);
      });
  },
  async getSession(): Promise<Credencials | false> {
    return axiosClient.get('/Auth/RefreshToken').then((response) => {
      if (response.status !== 200) {
        throw new Error('Não Autorizado');
      }
      const body = response.data as LoginResponseBody;
      return body.credentials ? body.credentials : false;
    });
  },
};
