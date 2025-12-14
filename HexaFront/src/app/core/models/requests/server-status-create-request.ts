import { ServerStatus } from "../enums/server-status";

export interface ServerStatusCreateRequest {
  serverId: string;
  status: ServerStatus;
}
