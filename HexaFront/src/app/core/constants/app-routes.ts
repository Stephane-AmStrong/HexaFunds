export const APP_ROUTES = {
  DASHBOARD: 'dashboard',
  ALERTS: {
    ROOT: 'alerts',
    LIST: '',
  },
  CLIENTS: {
    ROOT: 'clients',
    LIST: '',
    DETAIL: ':id',
  },
  CONNECTIONS: {
    ROOT: 'connections',
    LIST: '',
    DETAIL: ':id',
  },
  HOSTS: {
    ROOT: 'hosts',
    LIST: '',
    DETAIL: ':hostName',
    SERVERS_REDIRECT: ':hostName/servers',
    SERVER_BY_HOST: ':hostName/servers/:serverName',
  },
  SERVERS: {
    ROOT: 'servers',
    LIST: '',
  },
  SERVERS_STATUS: {
    ROOT: 'server-statuses',
    LIST: '',
    DETAIL: ':id',
  },
  ROOT: '',
  ERRORS: {
    BAD_GATEWAY: 'bad-gateway',
    FORBIDDEN: 'forbidden',
    INTERNAL_SERVER_ERROR: 'internal-server-error',
    RATE_LIMIT: 'rate-limit',
    RESOURCE_NOT_FOUND: 'resource-not-found',
    ROOT: 'error',
    SERVICE_UNAVAILABLE: 'service-unavailable',
    UNKNOWN: 'unknown',
    UNAUTHORIZED: 'unauthorized',
  },
  NOT_FOUND: '**',
} as const;
