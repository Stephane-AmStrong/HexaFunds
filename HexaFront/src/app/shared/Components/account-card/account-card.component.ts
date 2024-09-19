import { Component, computed, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { CheckingAccountResponse } from '../../../features/banking/models/checking-account-response';
import { SavingsAccountResponse } from '../../../features/banking/models/savings-account-response';

@Component({
  selector: 'account-card',
  standalone: true,
  imports: [
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatRippleModule,
  ],
  templateUrl: './account-card.component.html',
  styleUrl: './account-card.component.scss',
})
export class AccountCardComponent {
  bankAccount = input.required<
    CheckingAccountResponse | SavingsAccountResponse
  >();

  checkingAccount = computed(() => {
    return (this.bankAccount() as CheckingAccountResponse).overdraftLimit !== undefined
      ? (this.bankAccount() as CheckingAccountResponse)
      : null;
  });

  savingsAccount = computed(() => {
    return (this.bankAccount() as SavingsAccountResponse).balanceCeiling !== undefined
      ? (this.bankAccount() as SavingsAccountResponse)
      : null;
  });

  balancePercentage = computed(() => {
    return Math.min(100, (this.bankAccount().balance / (this.checkingAccount() ? this.checkingAccount()?.overdraftLimit! : this.savingsAccount()?.balanceCeiling!)) * 100);
  });
}
