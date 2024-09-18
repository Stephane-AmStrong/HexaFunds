import { BankAccountRequest } from "./bank-account-request";

export interface BankAccountResponse extends BankAccountRequest {
    id: string;
}
