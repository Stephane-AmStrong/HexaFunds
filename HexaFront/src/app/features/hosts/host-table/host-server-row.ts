import { HostResponse } from "../../../core/models/responses/host-response";
import { ServerResponse } from "../../../core/models/responses/server-response";

export interface HostServerRow {
  host: HostResponse;
  server: ServerResponse;
  hostRowSpan: number;
  isFirstServer: boolean;
}
