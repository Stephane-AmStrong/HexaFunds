import { ConnectionResponse } from '../../core/models/responses/connection-response';
import { ServerStatusDetailedResponse } from '../../core/models/responses/server-status-detailed-response';

export const MOCK_SERVER_STATUSES: ServerStatusDetailedResponse[] = [
  {
    // Status 1 - Up
    id: 'STATUS-1',
    serverId: 'SERVER-1',
    status: "Up",
    recordedAt: '2025-08-20T11:58:00.000Z',
    server: {
      id: 'SERVER-1',
      hostName: 'DTCODEVMCSDX04W',
      appName: 'DgxEgm',
      port: '31000',
      type: 'mas-server',
      cronStartTime: '30 07 * * 1',
      cronStopTime: '00 22 * * 1',
      version: '1.0.0',
      tags: ['md-trayport-egm-uat.mcs.gem.myengie.com:31000'],
      latestStatus: {
        id: 'STATUS-1',
        serverId: 'SERVER-1',
        status: "Up",
        recordedAt: '2025-08-20T11:58:00.000Z',
      },
    },
  },
  {
    // Status 2 - Up
    id: 'STATUS-2',
    serverId: 'SERVER-2',
    status: "Up",
    recordedAt: '2025-08-20T11:59:30.000Z',
    server: {
      id: 'SERVER-2',
      hostName: 'DTCODEVMCSGT01W',
      appName: 'Gateway',
      port: '5432',
      type: 'database-server',
      cronStartTime: '00 06 * * *',
      cronStopTime: '00 23 * * *',
      version: '2.1.0',
      tags: ['postgresql-prod.db.myengie.com:5432', 'primary'],
      latestStatus: {
        id: 'STATUS-2',
        serverId: 'SERVER-2',
        status: "Up",
        recordedAt: '2025-08-20T11:59:30.000Z',
      },
    },
  },
  {
    // Status 3 - Down
    id: 'STATUS-3',
    serverId: 'SERVER-3',
    status: "Down",
    recordedAt: '2025-08-20T10:15:00.000Z',
    server: {
      id: 'SERVER-3',
      hostName: 'DTCODEVMCSHP01W',
      appName: 'Horsepower-Orders',
      port: '80',
      type: 'api-gateway',
      cronStartTime: '00 00 * * *',
      cronStopTime: '23 59 * * *',
      version: '3.0.1',
      tags: ['api-gateway.myengie.com:80', 'api-gateway.myengie.com:443'],
      latestStatus: {
        id: 'STATUS-3',
        serverId: 'SERVER-3',
        status: "Down",
        recordedAt: '2025-08-20T10:15:00.000Z',
      },
    },
  },
  {
    // Status 4 - Up
    id: 'STATUS-4',
    serverId: 'SERVER-4',
    status: "Up",
    recordedAt: '2025-08-20T11:59:45.000Z',
    server: {
      id: 'SERVER-4',
      hostName: 'Redis Cache Cluster',
      appName: 'Horsepower-Trades',
      port: '6379',
      type: 'cache-server',
      cronStartTime: '00 00 * * *',
      cronStopTime: '23 59 * * *',
      version: '7.2.0',
      tags: ['redis-cache.myengie.com:6379', 'node-1'],
      latestStatus: {
        id: 'STATUS-4',
        serverId: 'SERVER-4',
        status: "Up",
        recordedAt: '2025-08-20T11:59:45.000Z',
      },
    },
  },
  {
    // Status 5 - Up
    id: 'STATUS-5',
    serverId: 'SERVER-5',
    status: "Up",
    recordedAt: '2025-08-20T11:57:00.000Z',
    server: {
      id: 'SERVER-5',
      hostName: 'DTCODEVMCSMELW',
      appName: 'Feedchecker-TGE',
      port: '5672',
      type: 'message-queue',
      cronStartTime: '00 05 * * *',
      cronStopTime: '23 55 * * *',
      version: '3.12.0',
      tags: ['rabbitmq.myengie.com:5672', 'rabbitmq-mgmt.myengie.com:15672'],
      latestStatus: {
        id: 'STATUS-5',
        serverId: 'SERVER-5',
        status: "Up",
        recordedAt: '2025-08-20T11:57:00.000Z',
      },
    },
  },
];
