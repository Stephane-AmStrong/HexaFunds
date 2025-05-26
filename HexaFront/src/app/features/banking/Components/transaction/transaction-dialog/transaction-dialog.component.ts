import { Component, inject, input } from '@angular/core';
import {
  ReactiveFormsModule,
  FormsModule,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogConfig,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TransactionRequest } from '../../../models/transaction-request';
import { TransactionResponse } from '../../../models/transaction-response';
import { Observable } from 'rxjs';

@Component({
  selector: 'transaction-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
  ],
  templateUrl: './transaction-dialog.component.html',
  styleUrl: './transaction-dialog.component.scss',
})
export class TransactionDialogComponent {
  title = input<string>('Transaction Dialog');

  private formbuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<TransactionDialogComponent>);
  protected transaction = inject(MAT_DIALOG_DATA);

  formTransaction: FormGroup = this.formbuilder.group({
    accountId: [this.transaction.accountId, Validators.required],
    amount: [this.transaction.amount, Validators.required],
    type: [this.transaction.type, Validators.required],
  });

  close() {
    this.dialogRef.close();
  }

  onSubmit() {
    const changes = this.formTransaction.value;
    this.dialogRef.close(changes);
  }
}

export function openTransactionDialog(
  dialog: MatDialog,
  transaction: Partial<TransactionRequest>
): Observable<TransactionResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...transaction },
  };

  const dialogRef = dialog.open(TransactionDialogComponent, config);
  return dialogRef.afterClosed();
}

export function openTransactionDeleteConfirmationDialog(
  dialog: MatDialog,
  transaction: Partial<TransactionRequest>
): Observable<TransactionResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...transaction, deletion: true },
  };

  const dialogRef = dialog.open(TransactionDialogComponent, config);
  return dialogRef.afterClosed();
}
