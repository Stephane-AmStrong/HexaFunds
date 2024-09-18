import { CheckingAccountRequest } from './checking-account-request';

export interface CheckingAccountResponse extends CheckingAccountRequest {
  id: string;
  balance: number;
}
