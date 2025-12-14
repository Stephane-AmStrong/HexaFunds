import { AlertSeverity } from '../enums/alert-severity';
import { AlertType } from '../enums/alert-type';
import { QueryParameters } from '../paging/query-parameters';

export interface AlertQueryParameters extends QueryParameters {
  withServerId?: string;
  ofType?: AlertType;
  ofSeverity?: AlertSeverity;
  occurredBefore?: string;
  occurredAfter?: string;
}
