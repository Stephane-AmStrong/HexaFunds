import { Component, Inject, inject, input } from '@angular/core';
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
import { DialogComponent } from '../../../../../shared/Components/dialog/dialog.component';
import { CheckingAccountService } from '../../../services/checking-account.service';
import { CheckingAccountRequest } from '../../../models/checking-account-request';
import { Observable } from 'rxjs';
import { CheckingAccountResponse } from '../../../models/checking-account-response';

@Component({
  selector: 'checking-account-dialog',
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
  templateUrl: './checking-account-dialog.component.html',
  styleUrl: './checking-account-dialog.component.scss',
})
export class CheckingAccountDialogComponent {
  title = input<string>('Checking Account Dialog');

  private formbuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CheckingAccountDialogComponent>);
  private checkingAccountService = inject(CheckingAccountService);
  protected checkingAccount = inject(MAT_DIALOG_DATA);

  formCheckingAccount: FormGroup = this.formbuilder.group({
    id: [this.checkingAccount.id],
    accountNumber: [
      {
        value: this.checkingAccount.accountNumber,
        disabled: this.checkingAccount.deletion,
      },
      Validators.required,
    ],
    overdraftLimit: [
      {
        value: this.checkingAccount.overdraftLimit,
        disabled: this.checkingAccount.deletion,
      },
      Validators.required,
    ],
  });

  close() {
    this.dialogRef.close();
  }

  save() {
    const changes = this.formCheckingAccount.value;
    if (!changes.id) {
      this.checkingAccountService
        .create(changes)
        .subscribe((newCheckingAccount) =>
          this.dialogRef.close(newCheckingAccount)
        );
    } else {
      this.checkingAccountService
        .update(changes.id, changes)
        .subscribe((updatedCheckingAccount) =>
          this.dialogRef.close(updatedCheckingAccount)
        );
    }
  }

  delete() {
    const changes = this.formCheckingAccount.value;

    this.checkingAccountService
      .delete(changes.id)
      .subscribe((deletedCheckingAccount) =>
        this.dialogRef.close(deletedCheckingAccount)
      );
  }
}

export function openCheckingAccountDialog(
  dialog: MatDialog,
  checkingAccount: Partial<CheckingAccountRequest>
): Observable<CheckingAccountResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...checkingAccount },
  };

  const dialogRef = dialog.open(CheckingAccountDialogComponent, config);
  return dialogRef.afterClosed();
}

export function openCheckingAccountDeleteConfirmationDialog(
  dialog: MatDialog,
  checkingAccount: Partial<CheckingAccountRequest>
): Observable<CheckingAccountResponse> {
  const config: MatDialogConfig = {
    disableClose: true,
    autoFocus: true,
    minWidth: 500,
    data: { ...checkingAccount, deletion: true },
  };

  const dialogRef = dialog.open(CheckingAccountDialogComponent, config);
  return dialogRef.afterClosed();
}
