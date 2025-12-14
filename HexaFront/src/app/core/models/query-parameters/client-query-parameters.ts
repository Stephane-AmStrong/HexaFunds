import { QueryParameters } from '../paging/query-parameters';

export interface ClientQueryParameters extends QueryParameters {
  withGaia?: string;
  withLogin?: string;
}