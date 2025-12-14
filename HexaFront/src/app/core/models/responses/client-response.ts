import { BaseModel } from '../base-model';

export interface ClientResponse extends BaseModel {
  login: string;
  gaia: string;
}
