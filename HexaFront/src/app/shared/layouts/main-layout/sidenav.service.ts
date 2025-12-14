import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { computed, inject, Injectable, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatSidenav } from '@angular/material/sidenav';
import { Observable, map, shareReplay } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SidenavService {
  private breakpointObserver = inject(BreakpointObserver);
  private sidenav = signal<MatSidenav | null>(null);

  isHandset = toSignal(
    this.breakpointObserver.observe(Breakpoints.Handset).pipe(map(r => r.matches)),
    { initialValue: false }
  );

  register(sidenav: MatSidenav) {
    this.sidenav.set(sidenav);
  }

  unregister() {
    this.sidenav.set(null);
  }

  isRegistered = computed(() => this.sidenav() !== null);
  isOpen = computed(() => !!this.sidenav()?.opened);

  toggle() {
    this.sidenav()?.toggle();
  }
  open() {
    this.sidenav()?.open();
  }
  close() {
    this.sidenav()?.close();
  }
}
