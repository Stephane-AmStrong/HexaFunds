import { ClientUpdateRequest } from "./client-update-request";

export interface ClientCreateRequest extends ClientUpdateRequest {
    id: string;
}
