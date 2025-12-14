import { ClientResponse } from './client-response';
import { ConnectionResponse } from './connection-response';
import { ServerResponse } from './server-response';

export interface ConnectionDetailedResponse extends ConnectionResponse {
  client: ClientResponse;
  server: ServerResponse;
}