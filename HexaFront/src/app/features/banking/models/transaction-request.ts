import { TransactionType } from './transaction-type';

export interface TransactionRequest {
  id?: string;
  amount: number;
  type: TransactionType;
  accountId: string;
  date: Date;
}
