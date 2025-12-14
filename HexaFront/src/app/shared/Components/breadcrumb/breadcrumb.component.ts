import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter, map, startWith, Subject, takeUntil } from 'rxjs';
import { MatListModule } from '@angular/material/list';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';

interface Breadcrumb {
  label: string;
  url: string;
}

@Component({
  selector: 'breadcrumb',
  imports: [MatButtonModule, MatIconModule, RouterLink, MatListModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
})
export class BreadcrumbComponent {
  #router = inject(Router);

  breadcrumbs = toSignal(
    this.#router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map(() => this.#buildBreadcrumbs()),
      startWith(this.#buildBreadcrumbs()),
      takeUntilDestroyed()
    ),
    { initialValue: [] }
  );

  #buildBreadcrumbs(): Breadcrumb[] {
    const breadcrumbs: Breadcrumb[] = [];

    const urlWithoutParams = this.#router.url.split('?')[0];
    const urlSegments = urlWithoutParams.split('/').filter(Boolean);

    let currentUrl = '';
    for (const segment of urlSegments) {
      currentUrl += `/${segment}`;
      breadcrumbs.push({
        label: this.#formatLabel(segment),
        url: currentUrl,
      });
    }

    return breadcrumbs;
  }

  #formatLabel(segment: string): string {
    return segment.replace(/-/g, ' ').replace(/\b\w/g, (l) => l.toUpperCase());
  }
}
