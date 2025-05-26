import { TransactionRequest } from './transaction-request';

export interface TransactionResponse extends TransactionRequest {
  id: string;
}
