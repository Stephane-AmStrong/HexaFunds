import { HttpHeaders } from "@angular/common/http";

export interface RequestOptions<TBody = unknown> {
  body?: TBody;
  params?: Record<string, string | number | boolean | undefined | null> | { [key: string]: any };
  headers?: HttpHeaders | Record<string, string | string[]>;
}