import { BaseModel } from '../base-model';
import { ServerStatus } from '../enums/server-status';

export interface ServerStatusResponse extends BaseModel {
  serverId: string;
  status: ServerStatus;
  recordedAt: string;
}
