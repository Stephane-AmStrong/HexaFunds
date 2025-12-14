import { BaseModel } from '../base-model';
import { AlertSeverity } from '../enums/alert-severity';
import { AlertStatus } from '../enums/alert-status';
import { AlertType } from '../enums/alert-type';

export interface AlertResponse extends BaseModel {
  serverId?: string;
  type?: AlertType;
  message?: string;
  severity?: AlertSeverity;
  occurrence: number;
  status: AlertStatus;
  occurredAt: string;
}
