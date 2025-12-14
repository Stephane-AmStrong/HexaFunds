import { AlertSeverity } from "../enums/alert-severity";
import { AlertType } from "../enums/alert-type";

export interface AlertCreateOrIncrementRequest {
  serverId?: string;
  type?: AlertType;
  message?: string;
  severity?: AlertSeverity;
}
