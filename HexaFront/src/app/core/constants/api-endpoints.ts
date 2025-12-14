export const API_ENDPOINTS = {
  ALERTS: {
    ROOT: 'alerts',
    BY_ID: (id: string) => `${API_ENDPOINTS.ALERTS.ROOT}/${id}`,
    LIVE: () => `${API_ENDPOINTS.ALERTS.ROOT}/live`
  },
  DASHBOARD: {
    ROOT: 'dashboard',
  },
  CLIENTS: {
    ROOT: 'clients',
    BY_ID: (id: string) => `${API_ENDPOINTS.CLIENTS.ROOT}/${id}`,
    LIVE: () => `${API_ENDPOINTS.CLIENTS.ROOT}/live`
  },
  CONNECTIONS: {
    ROOT: 'connections',
    BY_ID: (id: string) => `${API_ENDPOINTS.CONNECTIONS.ROOT}/${id}`,
    LIVE: () => `${API_ENDPOINTS.CONNECTIONS.ROOT}/live`
  },
  HOSTS: {
    ROOT: 'hosts',
    BY_ID: (id: string) => `${API_ENDPOINTS.HOSTS.ROOT}/by-id/${id}`,
    BY_NAME: (name: string) => `${API_ENDPOINTS.HOSTS.ROOT}/${name}`,
    WITH_SERVERS: () => `${API_ENDPOINTS.HOSTS.ROOT}/with-servers`,
    SERVER_APP: (hostName: string, appName: string) => `${API_ENDPOINTS.HOSTS.ROOT}/${hostName}/servers/${appName}`,
    LIVE: () => `${API_ENDPOINTS.HOSTS.ROOT}/live`
  },
  SERVERS: {
    ROOT: 'servers',
    BY_ID: (id: string) => `${API_ENDPOINTS.SERVERS.ROOT}/${id}`,
    LIVE: () => `${API_ENDPOINTS.SERVERS.ROOT}/live`
  },
  SERVER_STATUSES: {
    ROOT: 'server-statuses',
    BY_ID: (id: string) => `${API_ENDPOINTS.SERVER_STATUSES.ROOT}/${id}`,
    LIVE: () => `${API_ENDPOINTS.SERVER_STATUSES.ROOT}/live`
  },
} as const;
