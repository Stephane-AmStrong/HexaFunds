import {
  AfterViewInit,
  Component,
  input,
  ViewChild,
} from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { CheckingAccountResponse } from '../../models/checking-account-response';
import { AccountCardComponent } from '../../../../shared/Components/account-card/account-card.component';
import { TransactionResponse } from '../../models/transaction-response';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTable, MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'checking-account-detail',
  standalone: true,
  imports: [
    MatGridListModule,
    AccountCardComponent,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './checking-account-detail.component.html',
  styleUrl: './checking-account-detail.component.scss',
})
export class CheckingAccountDetailComponent implements AfterViewInit {
  @ViewChild(MatTable) table!: MatTable<TransactionResponse>;

  checkingAccount = input.required<CheckingAccountResponse>();
  transactions = input.required<TransactionResponse[]>();

  displayedColumns = ['type', 'amount', 'date'];

  ngAfterViewInit(): void {
    this.table.dataSource = this.transactions();
  }
}
