import { PagingMetadata } from "./paging-metadata";

export interface PagedList<T> {
  data: T[];
  metaData: PagingMetadata;
}
