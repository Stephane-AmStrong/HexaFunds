import { QueryParameters } from '../paging/query-parameters';

export interface ConnectionQueryParameters extends QueryParameters {
  withClientId?: string;
  withServerId?: string;
  withApplication?: string;
}