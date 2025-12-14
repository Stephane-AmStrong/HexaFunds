import { ClientResponse } from "./client-response";
import { ConnectionResponse } from "./connection-response";

export interface ClientDetailedResponse extends ClientResponse {
  connections: ConnectionResponse[];
}