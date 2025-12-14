export interface ConnectionEstablishRequest {
  clientId: string;
  serverId: string;
  application: string;
  apiVersion: string;
  machine: string;
  processId: string;
}
