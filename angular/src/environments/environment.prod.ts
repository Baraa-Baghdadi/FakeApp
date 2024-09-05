import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'DawaaNeo',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44385/',
    redirectUri: baseUrl,
    clientId: 'DawaaNeo_App',
    responseType: 'code',
    scope: 'offline_access DawaaNeo',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44385',
      rootNamespace: 'DawaaNeo',
    },
  },
} as Environment;
