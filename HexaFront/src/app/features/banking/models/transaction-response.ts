import { TransactionType } from './transaction-type';

export interface TransactionResponse {
  id: string;
  date: Date;
  amount: number;
  type: TransactionType;
}
