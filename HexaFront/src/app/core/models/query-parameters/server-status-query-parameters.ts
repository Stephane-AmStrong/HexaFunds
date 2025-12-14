import { ServerStatus } from '../enums/server-status';
import { QueryParameters } from '../paging/query-parameters';

export interface ServerStatusQueryParameters extends QueryParameters {
  withServerId?: string;
  ofStatus?: ServerStatus;
  recordedBefore?: string;
  recordedAfter?: string;
}