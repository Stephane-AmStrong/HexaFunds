import { ServerUpdateRequest } from './server-update-request';

export interface ServerCreateRequest extends ServerUpdateRequest {
  id: string;
}
