import { HostResponse } from "./host-response";
import { ServerResponse } from "./server-response";

export interface HostDetailedResponse extends HostResponse {
    servers : ServerResponse[];
}
