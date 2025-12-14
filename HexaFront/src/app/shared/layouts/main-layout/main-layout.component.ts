import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { afterNextRender, Component, inject, OnDestroy, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Observable, map, shareReplay } from 'rxjs';
import { SidenavService } from './sidenav.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'main-layout',
  imports: [
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    AsyncPipe,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
})
export class MainLayoutComponent implements OnDestroy {
  drawer = viewChild.required<MatSidenav>('drawer');

  private breakpointObserver = inject(BreakpointObserver);

  private sidenavService = inject(SidenavService);

  constructor() {
    afterNextRender(() => {
      this.sidenavService.register(this.drawer());
    });
  }

  hostQueryParams = {
    page: 1,
    pageSize: 10,
    orderBy: 'name'
  };

  serverQueryParams = {
    page: 1,
    pageSize: 10,
    orderBy: 'appName'
  };

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay()
  );

  ngOnDestroy(): void {
    this.sidenavService.unregister();
  }
}
