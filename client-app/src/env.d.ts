/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_BACKEND: string;
  readonly VITE_BACKEND_DEV: string;
}

interface ImportMeta {
  readonly emv: string;
}
