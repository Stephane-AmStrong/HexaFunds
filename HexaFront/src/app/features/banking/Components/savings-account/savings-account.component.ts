import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { AccountCardComponent } from '../../../../shared/Components/account-card/account-card.component';

@Component({
  selector: 'savings-account',
  standalone: true,
  imports: [AccountCardComponent, MatGridListModule],
  templateUrl: './savings-account.component.html',
  styleUrl: './savings-account.component.scss',
})
export class SavingsAccountComponent {
  cards = Array.from({ length: 100 }, (_, i) => ({
    title: `Card ${i + 1}`,
    cols: (i % 5) + 1,
    rows: (i % 5) + 1,
  }));
}
