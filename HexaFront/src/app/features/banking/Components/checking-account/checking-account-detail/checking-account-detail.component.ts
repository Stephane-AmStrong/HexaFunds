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
import { openDialog } from '../../../../../shared/Components/dialog/dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { DatePipe } from '@angular/common';
import {
  openCheckingAccountDeleteConfirmationDialog,
  openCheckingAccountDialog,
} from '../checking-account-dialog/checking-account-dialog.component';
import { Router } from '@angular/router';

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
  checkingAccount = model.required<CheckingAccountResponse>();
  transactions = model.required<TransactionResponse[]>();

  newTransaction = signal<TransactionResponse | null>(null);

  displayedColumns = ['type', 'amount', 'date'];

  edit() {
    openCheckingAccountDialog(this.dialog, {
      ...this.checkingAccount(),
    })
      .pipe(filter((val) => !!val))
      .subscribe();
  }

  delete() {
    openCheckingAccountDeleteConfirmationDialog(this.dialog, {
      ...this.checkingAccount(),
    })
      .pipe(filter((val) => !!val))
      .subscribe(() => this.router.navigate(['/checking-accounts']));
  }

  addTransaction() {
    openDialog(this.dialog, {
      accountId: this.checkingAccount().id,
    })
      .pipe(filter((val) => !!val))
      .subscribe((transaction) =>
        this.transactions.update((currentTransactions) => [
          ...currentTransactions,
          transaction,
        ])
      );
  }
}
