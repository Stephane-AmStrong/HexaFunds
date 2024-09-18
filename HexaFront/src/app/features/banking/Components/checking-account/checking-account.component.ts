import { Component, inject } from '@angular/core';
import { AccountCardComponent } from '../../../../shared/Components/account-card/account-card.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { toSignal } from '@angular/core/rxjs-interop';
import { CheckingAccountResponse } from '../../models/checking-account-response';
import { CheckingAccountService } from '../../services/checking-account.service';

@Component({
  selector: 'checking-account',
  standalone: true,
  imports: [AccountCardComponent, MatGridListModule],
  templateUrl: './checking-account.component.html',
  styleUrl: './checking-account.component.scss',
})
export class CheckingAccountComponent {

  private checkingAccountService = inject(CheckingAccountService);

  checkingAccounts = toSignal<
    CheckingAccountResponse[],
    CheckingAccountResponse[]
  >(this.checkingAccountService.getAll(), { initialValue: [] });
}
