import Cookies from 'universal-cookie';

const ACCESS_TOKEN_KEY = 'QSATKSO';

const ONE_YEAR_IN_SECONDS = 60 * 60 * 24 * 360;

const cookies = new Cookies();

export const tokenService = {
  save(accessToken: string) {
    globalThis?.localStorage?.setItem(ACCESS_TOKEN_KEY, accessToken);
    cookies.set(ACCESS_TOKEN_KEY, accessToken, {
      maxAge: ONE_YEAR_IN_SECONDS,
      path: '/',
    });
  },
  get() {
    const cookiesRestored = cookies.get(ACCESS_TOKEN_KEY);
    return cookiesRestored || '';
    // return globalThis?.localStorage?.getItem(ACCESS_TOKEN_KEY);
  },
  async delete() {
    await cookies.remove(ACCESS_TOKEN_KEY);
    // globalThis?.localStorage?.removeItem(ACCESS_TOKEN_KEY);
  },
};
