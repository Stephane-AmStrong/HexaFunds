import { BankAccountRequest } from "./bank-account-request";

export interface CheckingAccountRequest extends BankAccountRequest{
    overdraftLimit: number
}
