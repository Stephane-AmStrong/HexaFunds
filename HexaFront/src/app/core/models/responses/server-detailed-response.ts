import { ConnectionResponse } from "./connection-response";
import { ServerResponse } from "./server-response";
import { ServerStatusResponse } from "./server-status-response";

export interface ServerDetailedResponse extends ServerResponse {
  connections: ConnectionResponse[];
  statuses: ServerStatusResponse[];
}
