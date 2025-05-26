import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { TransactionResponse } from '../../../models/transaction-response';
import { CommonModule, DatePipe } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { TransactionType } from '../../../models/transaction-type';

@Component({
  selector: 'transaction-card',
  standalone: true,
  imports: [
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatRippleModule,
    DatePipe,
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
  ],
  templateUrl: './transaction-card.component.html',
  styleUrl: './transaction-card.component.scss',
})
export class TransactionCardComponent {
  transaction = input.required<TransactionResponse>();
  getTransactionTypeClass(): string {
    return this.transaction().type === 'Credit'
      ? 'credit-transaction'
      : 'debit-transaction';
  }
}
