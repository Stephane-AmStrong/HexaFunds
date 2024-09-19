import { BankAccountRequest } from "./bank-account-request";

export interface SavingsAccountRequest extends BankAccountRequest{
  balanceCeiling: number;
}
