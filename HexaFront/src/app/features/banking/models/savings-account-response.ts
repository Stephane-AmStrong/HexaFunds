import { SavingsAccountRequest } from './savings-account-request';

export interface SavingsAccountResponse extends SavingsAccountRequest {
  id: string;
  balance: number;
}
