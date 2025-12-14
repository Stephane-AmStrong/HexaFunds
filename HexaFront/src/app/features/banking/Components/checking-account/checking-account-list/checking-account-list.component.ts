import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { AccountCardComponent } from '../../../../../shared/Components/account-card/account-card.component';
import { CheckingAccountsService } from '../../../services/checking-account.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { CheckingAccountResponse } from '../../../models/checking-account-response';
import { openCheckingAccountDialog } from '../checking-account-dialog/checking-account-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { CheckingAccountRequest } from '../../../models/checking-account-request';

@Component({
  selector: 'checking-account-list',
  standalone: true,
  imports: [
    MatIconModule,
    MatButtonModule,
    AccountCardComponent,
    MatGridListModule,
    RouterLink,
  ],
  templateUrl: './checking-account-list.component.html',
  styleUrl: './checking-account-list.component.scss',
})
export class CheckingAccountListComponent {
  private checkingAccountsService = inject(CheckingAccountsService);
  private dialog = inject(MatDialog);

  checkingAccounts = toSignal<
    CheckingAccountResponse[],
    CheckingAccountResponse[]
  >(this.checkingAccountsService.getAll(), {
    initialValue: [],
  });

  addCheckingAccount() {
    openCheckingAccountDialog(this.dialog, {})
      .pipe(filter((checkingAccountRequest) => !!checkingAccountRequest))
      .subscribe((checkingAccountRequest) => {
        this.create(checkingAccountRequest);
      });
  }

  create(checkingAccountRequest: CheckingAccountRequest) {
    if (!checkingAccountRequest.id) {
      this.checkingAccountsService
        .create(checkingAccountRequest)
        .subscribe((newCheckingAccount) =>
          console.log(
            'New checking account created successfully:'
            // newCheckingAccount
          )
        );
    }
  }
}
