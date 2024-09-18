import { TransactionResponse } from "./transaction-response";

export interface AccountTransactionsResponse {
    accountType: string;
    balance: number;
    transactions: TransactionResponse[]
}
