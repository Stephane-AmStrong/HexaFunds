import { Component, inject } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs/operators';
import { AsyncPipe } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { SectionMonitoringOverviewComponent } from "../sections/section-monitoring-overview/section-monitoring-overview.component";
import { SectionApplicationsComponent } from "../sections/section-applications/section-applications.component";
import { SectionServicesComponent } from "../sections/section-services/section-services.component";
import { SectionInfrastructureComponent } from "../sections/section-infrastructure/section-infrastructure.component";
import { ToolbarComponent } from "../../../shared/components/toolbar/toolbar.component";

@Component({
  selector: 'layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
  imports: [
    AsyncPipe,
    MatGridListModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    SectionMonitoringOverviewComponent,
    SectionApplicationsComponent,
    SectionServicesComponent,
    SectionInfrastructureComponent,
    ToolbarComponent
]
})
export class LayoutComponent {
  private breakpointObserver = inject(BreakpointObserver);
  /** Based on the screen size, switch from standard to one column per row */
  dashboardLayout = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [
          { title: 'Card 1', rows: 1, cols: 1 },
          { title: 'Card 2', rows: 1, cols: 1 },
          { title: 'Card 3', rows: 1, cols: 1 },
          { title: 'Card 4', rows: 1, cols: 1 },
          { title: 'Card 5', rows: 1, cols: 1 }
        ];
      }

      return [
        { title: 'Problems', rows: 4, cols: 7 },
        { title: 'Services', rows: 4, cols: 19 },
        { title: 'Applications', rows: 4, cols: 19 },
        { title: 'Infrastructure', rows: 4, cols: 19 },
      ];
    })
  );
}
/*
      <mat-grid-tile-header>Problems</mat-grid-tile-header>
      <mat-grid-tile-header>Services</mat-grid-tile-header>
      <mat-grid-tile-header>Applications</mat-grid-tile-header>
      <mat-grid-tile-header>Infrastructure</mat-grid-tile-header>

*/