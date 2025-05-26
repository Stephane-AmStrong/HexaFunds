import { Component, inject, signal, ViewChild, model } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { SavingsAccountResponse } from '../../../models/savings-account-response';
import { TransactionResponse } from '../../../models/transaction-response';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTable, MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { DatePipe } from '@angular/common';
import {
  openSavingsAccountDeleteConfirmationDialog,
  openSavingsAccountDialog,
} from '../savings-account-dialog/savings-account-dialog.component';
import { Router } from '@angular/router';
import { SavingsAccountRequest } from '../../../models/savings-account-request';
import { SavingsAccountService } from '../../../services/savings-account.service';
import { openTransactionDialog } from '../../transaction/transaction-dialog/transaction-dialog.component';
import { TransactionService } from '../../../services/transaction.service';
import { TransactionRequest } from '../../../models/transaction-request';

@Component({
  selector: 'savings-account-detail',
  standalone: true,
  imports: [
    MatGridListModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    DatePipe,
  ],
  templateUrl: './savings-account-detail.component.html',
  styleUrl: './savings-account-detail.component.scss',
})
export class SavingsAccountDetailComponent {
  @ViewChild(MatTable) table!: MatTable<TransactionResponse>;

  private router = inject(Router);
  private dialog = inject(MatDialog);

  private savingsAccountService = inject(SavingsAccountService);
  private transactionService = inject(TransactionService);

  savingsAccount = model.required<SavingsAccountResponse>();
  transactions = model.required<TransactionResponse[]>();

  newTransaction = signal<TransactionResponse | null>(null);

  displayedColumns = ['type', 'amount', 'date'];

  editSavingsAccount() {
    openSavingsAccountDialog(this.dialog, {
      ...this.savingsAccount(),
    })
      .pipe(filter((savingsAccountResponse) => !!savingsAccountResponse))
      .subscribe((savingsAccountResponse) => {
        this.save(savingsAccountResponse);
      });
  }

  deleteSavingsAccount() {
    openSavingsAccountDeleteConfirmationDialog(this.dialog, {
      ...this.savingsAccount(),
    })
      .pipe(filter((savingsAccountRequest) => !!savingsAccountRequest))
      .subscribe((savingsAccountReponse) => this.delete(savingsAccountReponse));
  }

  addTransaction() {
    openTransactionDialog(this.dialog, {
      accountId: this.savingsAccount().id,
    })
      .pipe(filter((TransactionResponse) => !!TransactionResponse))
      .subscribe((TransactionResponse) =>
        this.saveTransaction(TransactionResponse)
      );
  }

  save(savingsAccountRequest: SavingsAccountRequest) {
    if (savingsAccountRequest.id) {
      this.savingsAccountService
        .update(savingsAccountRequest.id, savingsAccountRequest)
        .subscribe(() =>
          this.savingsAccount.set({
            ...savingsAccountRequest,
            balance: this.savingsAccount().balance,
          } as SavingsAccountResponse)
        );
    }
  }

  delete(savingsAccountRequest: SavingsAccountRequest) {
    if (savingsAccountRequest.id) {
      this.savingsAccountService
        .delete(savingsAccountRequest.id)
        .subscribe(() => this.router.navigate(['/savings-accounts']));
    }
  }

  saveTransaction(transactionRequest: TransactionRequest) {
    this.transactionService
      .create(transactionRequest)
      .subscribe((newTransaction) => {
        this.transactions.update((currentTransactions) => [
          ...currentTransactions,
          newTransaction,
        ]);

        const newBalance =
          newTransaction.type === 'Credit'
            ? this.savingsAccount().balance + newTransaction.amount
            : this.savingsAccount().balance - newTransaction.amount;

        this.savingsAccount.update((account) => ({
          ...account,
          balance: newBalance,
        }));
      });
  }
}
