module.exports = {
  env: {
    browser: true,
    es2021: true,
    node: true,
  },
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:react-hooks/recommended',
    'plugin:prettier/recommended',
  ],
  overrides: [],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    project: './tsconfig.json',
  },
  plugins: ['react', '@typescript-eslint', 'prettier'],
  rules: {
    'react/react-in-jsx-scope': 0,
    'jsx-a11y/aria-role': 0,
    'prettier/prettier': 1,
    'require-default-props': 0,
    'import/no-extraneous-dependencies': 0,
    'react/require-default-props': 'off',
    'optional-object-property': 'off',
    'no-use-before-define': [
      1,
      {
        functions: false,
        classes: true,
        variables: true,
        allowNamedExports: false,
      },
    ],
  },
};
