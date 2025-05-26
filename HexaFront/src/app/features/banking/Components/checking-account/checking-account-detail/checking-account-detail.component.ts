import { Component, inject, signal, ViewChild, model } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { CheckingAccountResponse } from '../../../models/checking-account-response';
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
  openCheckingAccountDeleteConfirmationDialog,
  openCheckingAccountDialog,
} from '../checking-account-dialog/checking-account-dialog.component';
import { Router } from '@angular/router';
import { CheckingAccountRequest } from '../../../models/checking-account-request';
import { CheckingAccountService } from '../../../services/checking-account.service';
import { openTransactionDialog } from '../../transaction/transaction-dialog/transaction-dialog.component';
import { TransactionService } from '../../../services/transaction.service';
import { TransactionRequest } from '../../../models/transaction-request';

@Component({
  selector: 'checking-account-detail',
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
  templateUrl: './checking-account-detail.component.html',
  styleUrl: './checking-account-detail.component.scss',
})
export class CheckingAccountDetailComponent {
  @ViewChild(MatTable) table!: MatTable<TransactionResponse>;

  private router = inject(Router);
  private dialog = inject(MatDialog);

  private checkingAccountService = inject(CheckingAccountService);
  private transactionService = inject(TransactionService);

  checkingAccount = model.required<CheckingAccountResponse>();
  transactions = model.required<TransactionResponse[]>();

  newTransaction = signal<TransactionResponse | null>(null);

  displayedColumns = ['type', 'amount', 'date'];

  editCheckingAccount() {
    openCheckingAccountDialog(this.dialog, {
      ...this.checkingAccount(),
    })
      .pipe(filter((checkingAccountResponse) => !!checkingAccountResponse))
      .subscribe((checkingAccountResponse) => {
        this.save(checkingAccountResponse);
      });
  }

  deleteCheckingAccount() {
    openCheckingAccountDeleteConfirmationDialog(this.dialog, {
      ...this.checkingAccount(),
    })
      .pipe(filter((checkingAccountRequest) => !!checkingAccountRequest))
      .subscribe((checkingAccountReponse) =>
        this.delete(checkingAccountReponse)
      );
  }

  addTransaction() {
    openTransactionDialog(this.dialog, {
      accountId: this.checkingAccount().id,
    })
      .pipe(filter((TransactionResponse) => !!TransactionResponse))
      .subscribe((TransactionResponse) =>
        this.saveTransaction(TransactionResponse)
      );
  }

  save(checkingAccountRequest: CheckingAccountRequest) {
    if (checkingAccountRequest.id) {
      this.checkingAccountService
        .update(checkingAccountRequest.id, checkingAccountRequest)
        .subscribe(() =>
          this.checkingAccount.set({
            ...checkingAccountRequest,
            balance: this.checkingAccount().balance,
          } as CheckingAccountResponse)
        );
    }
  }

  delete(checkingAccountRequest: CheckingAccountRequest) {
    if (checkingAccountRequest.id) {
      this.checkingAccountService
        .delete(checkingAccountRequest.id)
        .subscribe(() => this.router.navigate(['/checking-accounts']));
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
            ? this.checkingAccount().balance + newTransaction.amount
            : this.checkingAccount().balance - newTransaction.amount;

        this.checkingAccount.update((account) => ({
          ...account,
          balance: newBalance,
        }));
      });
  }
}
