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
import { SavingsAccountRequest } from '../../../models/savings-account-request';
import { SavingsAccountResponse } from '../../../models/savings-account-response';
import { Observable } from 'rxjs';

@Component({
  selector: 'savings-account-dialog',
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
  templateUrl: './savings-account-dialog.component.html',
  styleUrl: './savings-account-dialog.component.scss',
})
export class SavingsAccountDialogComponent {
  title = input<string>('Savings Account Dialog');

  private formbuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<SavingsAccountDialogComponent>);
  protected savingsAccount = inject(MAT_DIALOG_DATA);

  formSavingsAccount: FormGroup = this.formbuilder.group({
    id: [this.savingsAccount.id],
    accountNumber: [
      {
        value: this.savingsAccount.accountNumber,
        disabled: this.savingsAccount.deletion,
      },
      Validators.required,
    ],
    balanceCeiling: [
      {
        value: this.savingsAccount.balanceCeiling,
        disabled: this.savingsAccount.deletion,
      },
      Validators.required,
    ],
  });

  close() {
    this.dialogRef.close();
  }

  onSubmit() {
    const changes = this.formSavingsAccount.value;
    this.dialogRef.close(changes);
  }
}

export function openSavingsAccountDialog(
  dialog: MatDialog,
  savingsAccount: Partial<SavingsAccountRequest>
): Observable<SavingsAccountResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...savingsAccount },
  };

  const dialogRef = dialog.open(SavingsAccountDialogComponent, config);
  return dialogRef.afterClosed();
}

export function openSavingsAccountDeleteConfirmationDialog(
  dialog: MatDialog,
  savingsAccount: Partial<SavingsAccountRequest>
): Observable<SavingsAccountResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...savingsAccount, deletion: true },
  };

  const dialogRef = dialog.open(SavingsAccountDialogComponent, config);
  return dialogRef.afterClosed();
}
