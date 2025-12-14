import { QueryParameters } from '../paging/query-parameters';

export interface HostQueryParameters extends QueryParameters {
  withName?: string;
}