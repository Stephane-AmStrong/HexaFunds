import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { openTransactionDialog } from '../transaction-dialog/transaction-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { TransactionRequest } from '../../../models/transaction-request';
import { TransactionResponse } from '../../../models/transaction-response';
import { TransactionService } from '../../../services/transaction.service';
import { TransactionCardComponent } from '../transaction-card/transaction-card.component';

@Component({
  selector: 'transaction-list',
  standalone: true,
  imports: [
    MatIconModule,
    MatButtonModule,
    TransactionCardComponent,
    MatGridListModule,
    RouterLink,
    TransactionCardComponent,
  ],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.scss',
})
export class TransactionListComponent {
  private transactionService = inject(TransactionService);
  private dialog = inject(MatDialog);

  transactions = toSignal<TransactionResponse[], TransactionResponse[]>(
    this.transactionService.getAll(),
    {
      initialValue: [],
    }
  );

  addTransaction() {
    openTransactionDialog(this.dialog, {})
      .pipe(filter((transactionRequest) => !!transactionRequest))
      .subscribe((transactionRequest) => {
        this.create(transactionRequest);
      });
  }

  create(transactionRequest: TransactionRequest) {
    if (!transactionRequest.id) {
      this.transactionService
        .create(transactionRequest)
        .subscribe((newTransaction) =>
          console.log(
            'New checking account created successfully:'
            // newTransaction
          )
        );
    }
  }
}
