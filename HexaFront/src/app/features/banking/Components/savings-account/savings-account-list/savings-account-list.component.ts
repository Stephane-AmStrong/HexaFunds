import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { AccountCardComponent } from '../../../../../shared/Components/account-card/account-card.component';
import { SavingsAccountService } from '../../../services/savings-account.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { SavingsAccountResponse } from '../../../models/savings-account-response';
import { openSavingsAccountDialog } from '../savings-account-dialog/savings-account-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { SavingsAccountRequest } from '../../../models/savings-account-request';

@Component({
  selector: 'savings-account-list',
  standalone: true,
  imports: [
    MatIconModule,
    MatButtonModule,
    AccountCardComponent,
    MatGridListModule,
    RouterLink,
  ],
  templateUrl: './savings-account-list.component.html',
  styleUrl: './savings-account-list.component.scss',
})
export class SavingsAccountListComponent {
  private savingsAccountService = inject(SavingsAccountService);
  private dialog = inject(MatDialog);

  savingsAccounts = toSignal<
    SavingsAccountResponse[],
    SavingsAccountResponse[]
  >(this.savingsAccountService.getAll(), {
    initialValue: [],
  });

  addSavingsAccount() {
    openSavingsAccountDialog(this.dialog, {})
      .pipe(filter((savingsAccountRequest) => !!savingsAccountRequest))
      .subscribe((savingsAccountRequest) => {
        this.create(savingsAccountRequest);
      });
  }

  create(savingsAccountRequest: SavingsAccountRequest) {
    if (!savingsAccountRequest.id) {
      this.savingsAccountService
        .create(savingsAccountRequest)
        .subscribe((newSavingsAccount) =>
          console.log(
            'New savings account created successfully:',
            newSavingsAccount
          )
        );
    }
  }
}
