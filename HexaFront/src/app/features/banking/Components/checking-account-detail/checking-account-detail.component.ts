import { Component, inject, input } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { CheckingAccountResponse } from '../../models/checking-account-response';
import { AccountCardComponent } from '../../../../shared/Components/account-card/account-card.component';

@Component({
  selector: 'checking-account-detail',
  standalone: true,
  imports: [MatGridListModule, AccountCardComponent],
  templateUrl: './checking-account-detail.component.html',
  styleUrl: './checking-account-detail.component.scss',
})
export class CheckingAccountDetailComponent {
  checkingAccount = input.required<CheckingAccountResponse>()
}
