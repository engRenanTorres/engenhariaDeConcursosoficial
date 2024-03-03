import axios, { AxiosError, AxiosResponse } from 'axios';
//import { tokenService } from '../../services/auth/tokenService';
import { PaginatedResult as PaginatedResult } from '../../interfaces/Pagination';
import { tokenService } from '../../services/auth/tokenService';

const axiosClient = axios.create({
  baseURL: `${import.meta.env.VITE_BACKEND_DEV}/api`,
  // baseURL: `${import.meta.env.VITE_BACKEND_DEV}/api`,
});

axiosClient.interceptors.response.use(
  async (response) => {
    const pagination = response.headers['pagination'];
    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<unknown>>;
    }
    return response;
  },
  (error: AxiosError) => {
    console.log(error);
  }
);

axiosClient.interceptors.request.use((config) => {
  try {
    const token = tokenService.get();

    if (token != null && config?.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  } catch (error) {
    throw new Error('Erro na injeção de token');
  }
});

export default axiosClient;
