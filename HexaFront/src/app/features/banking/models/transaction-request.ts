import { TransactionType } from './transaction-type';

export interface TransactionRequest {
  amount: number;
  type: TransactionType;
  accountId: string;
  date: Date;
}
