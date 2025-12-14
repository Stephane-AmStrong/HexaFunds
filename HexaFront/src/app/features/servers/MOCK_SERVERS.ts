import { ConnectionResponse } from '../../core/models/responses/connection-response';
import { ServerDetailedResponse } from '../../core/models/responses/server-detailed-response';
import { ServerStatusResponse } from '../../core/models/responses/server-status-response';

export const MOCK_SERVERS: ServerDetailedResponse[] = [
  {
    // Server 1 - Trayport Market Data
    id: 'SERVER-1',
    hostName: 'Trayport',
    version: '1.0.0',
    port: '31000',
    appName: 'TrayportMarketDataEgm',
    type: 'mas-server',
    cronStartTime: '30 07 * * 1',
    cronStopTime: '00 22 * * 1',
    tags: ['md-trayport-egm-uat','mcs.gem.myengie.com:31000'],
    latestStatus: {
      id: 'SERVERSTATUS-LAST-1',
      recordedAt: '2025-08-20T11:58:00.000Z',
      serverId: 'SERVER-1',
      status: 'Up',
    } as ServerStatusResponse,
    connections: [
      {
        id: 'CONNECTION-1A',
        clientId: 'CLIENT-TRAY-1',
        serverId: 'SERVER-1',
        application: 'dotnet',
        apiVersion: '1.0.0',
        machine: 'trayport-machine-1',
        processId: '1234',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-1A',
        streak: 5,
        recordedAt: '2025-08-20T11:58:00.000Z',
        serverId: 'SERVER-1',
        status: 'Up',
      } as ServerStatusResponse,
    ],
  },
  {
    // Server 2 - PostgreSQL Database
    id: 'SERVER-2',
    hostName: 'DatabaseServer',
    version: '2.1.0',
    port: '5432',
    appName: 'PostgreSQLService',
    type: 'database-server',
    cronStartTime: '00 06 * * *',
    cronStopTime: '00 23 * * *',
    tags: ['postgresql-prod.db.myengie.com:5432'],
    latestStatus: {
      id: 'SERVERSTATUS-LAST-2',
      streak: 25,
      recordedAt: '2025-08-20T11:59:30.000Z',
      serverId: 'SERVER-2',
      status: 'Up',
    } as ServerStatusResponse,
    connections: [
      {
        id: 'CONNECTION-2A',
        clientId: 'CLIENT-DB-1',
        serverId: 'SERVER-2',
        application: 'postgresql',
        apiVersion: '2.1.0',
        machine: 'db-machine-1',
        processId: '5678',
      } as ConnectionResponse,
      {
        id: 'CONNECTION-2B',
        clientId: 'CLIENT-DB-2',
        serverId: 'SERVER-2',
        application: 'postgresql',
        apiVersion: '2.1.0',
        machine: 'db-machine-2',
        processId: '5679',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-2A',
        streak: 8,
        recordedAt: '2025-08-20T11:59:30.000Z',
        serverId: 'SERVER-2',
        status: 'Up',
      } as ServerStatusResponse,
    ],
  },
  {
    // Server 3 - API Gateway (Problématique)
    id: 'SERVER-3',
    hostName: 'APIGateway test test',
    version: '3.0.1',
    port: '80',
    appName: 'NginxAPIGateway',
    type: 'api-gateway',
    cronStartTime: '00 00 * * *',
    cronStopTime: '23 59 * * *',
    tags: ['api-gateway.myengie.com:80', 'api-gateway.myengie.com:443'],
    latestStatus: {
      id: 'SERVERSTATUS-3A',
      recordedAt: '2025-08-20T10:15:00.000Z',
      serverId: 'SERVER-3',
      status: 'Down',
    },
    connections: [
      {
        id: 'CONNECTION-3A',
        clientId: 'CLIENT-GATEWAY-1',
        serverId: 'SERVER-3',
        application: 'nginx',
        apiVersion: '3.0.1',
        machine: 'gateway-1',
        processId: '9999',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-3A',
        recordedAt: '2025-08-20T10:15:00.000Z',
        serverId: 'SERVER-3',
        status: 'Down',
      } as ServerStatusResponse,
    ],
  },

  {
    // Server 4 - Redis Cache
    id: 'SERVER-4',
    hostName: 'RedisCache',
    version: '7.2.0',
    port: '6379',
    appName: 'RedisCacheService',
    type: 'cache-server',
    cronStartTime: '00 00 * * *',
    cronStopTime: '23 59 * * *',
    tags: ['redis-cache.myengie.com:6379'],
    latestStatus: {
      id: 'SERVERSTATUS-LAST-4',
      streak: 42,
      recordedAt: '2025-08-20T11:59:45.000Z',
      serverId: 'SERVER-4',
      status: 'Up',
    } as ServerStatusResponse,
    connections: [
      {
        id: 'CONNECTION-4A',
        clientId: 'CLIENT-CACHE-1',
        serverId: 'SERVER-4',
        application: 'redis',
        apiVersion: '7.2.0',
        machine: 'cache-machine-1',
        processId: '7777',
      } as ConnectionResponse,
      {
        id: 'CONNECTION-4B',
        clientId: 'CLIENT-CACHE-2',
        serverId: 'SERVER-4',
        application: 'redis',
        apiVersion: '7.2.0',
        machine: 'cache-machine-2',
        processId: '7778',
      } as ConnectionResponse,
      {
        id: 'CONNECTION-4C',
        clientId: 'CLIENT-CACHE-3',
        serverId: 'SERVER-4',
        application: 'redis',
        apiVersion: '7.2.0',
        machine: 'cache-machine-3',
        processId: '7779',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-4A',
        streak: 12,
        recordedAt: '2025-08-20T11:59:45.000Z',
        serverId: 'SERVER-4',
        status: 'Up',
      } as ServerStatusResponse,
    ],
  },

  {
    // Server 5 - Message Queue (RabbitMQ)
    id: 'SERVER-5',
    hostName: 'MessageQueue',
    version: '3.12.0',
    port: '5672',
    appName: 'RabbitMQService',
    type: 'message-queue',
    cronStartTime: '00 05 * * *',
    cronStopTime: '23 55 * * *',
    tags: ['rabbitmq.myengie.com:5672', 'rabbitmq-mgmt.myengie.com:15672'],
    latestStatus: {
      id: 'SERVERSTATUS-LAST-5',
      streak: 3,
      recordedAt: '2025-08-20T11:57:00.000Z',
      serverId: 'SERVER-5',
      status: 'Up',
    } as ServerStatusResponse,
    connections: [
      {
        id: 'CONNECTION-5A',
        clientId: 'CLIENT-QUEUE-1',
        serverId: 'SERVER-5',
        application: 'rabbitmq',
        apiVersion: '3.12.0',
        machine: 'queue-machine-1',
        processId: '8888',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-5A',
        streak: 2,
        recordedAt: '2025-08-20T11:57:00.000Z',
        serverId: 'SERVER-5',
        status: 'Up',
      } as ServerStatusResponse,
      {
        id: 'SERVERSTATUS-5B',
        streak: 1,
        recordedAt: '2025-08-20T11:56:00.000Z',
        serverId: 'SERVER-5',
        status: 'Up',
      } as ServerStatusResponse,
    ],
  },

  {
    // Server 6 - Message Queue (RabbitMQ)
    id: 'SERVER-6',
    hostName: 'MessageQueue',
    version: '3.12.0',
    port: '5672',
    appName: 'RabbitMQService',
    type: 'message-queue',
    cronStartTime: '00 05 * * *',
    cronStopTime: '23 55 * * *',
    tags: ['rabbitmq.myengie.com:5672', 'rabbitmq-mgmt.myengie.com:15672'],
    latestStatus: {
      id: 'SERVERSTATUS-LAST-6',
      streak: 3,
      recordedAt: '2025-08-20T11:57:00.000Z',
      serverId: 'SERVER-6',
      status: 'Up',
    } as ServerStatusResponse,
    connections: [
      {
        id: 'CONNECTION-6A',
        clientId: 'CLIENT-QUEUE-1',
        serverId: 'SERVER-6',
        application: 'rabbitmq',
        apiVersion: '3.12.0',
        machine: 'queue-machine-1',
        processId: '8888',
      } as ConnectionResponse,
    ],
    statuses: [
      {
        id: 'SERVERSTATUS-5A',
        streak: 2,
        recordedAt: '2025-08-20T11:57:00.000Z',
        serverId: 'SERVER-5',
        status: 'Up',
      } as ServerStatusResponse,
      {
        id: 'SERVERSTATUS-5B',
        streak: 1,
        recordedAt: '2025-08-20T11:56:00.000Z',
        serverId: 'SERVER-5',
        status: 'Up',
      } as ServerStatusResponse,
    ],
  },
];
