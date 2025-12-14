import { AlertResponse } from "./alert-response";
import { ServerResponse } from "./server-response";

export interface AlertDetailedResponse extends AlertResponse {
  server: ServerResponse;
}
