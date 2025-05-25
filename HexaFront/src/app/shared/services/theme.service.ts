import { OverlayContainer } from '@angular/cdk/overlay';
import { Injectable, computed, effect, inject, signal } from '@angular/core';

type Theme = 'light' | 'dark';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private overlayContainer = inject(OverlayContainer);
  private readonly _currentTheme = signal<Theme>('light');

  constructor() {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    this._currentTheme.set(mediaQuery.matches ? 'dark' : 'light');

    mediaQuery.addEventListener('change', (e) =>
      this._currentTheme.set(e.matches ? 'dark' : 'light')
    );

    effect(() => this.updateOverlayClass(this._currentTheme()));
  }

  getTheme() {
    return computed(() => this._currentTheme());
  }

  setTheme(theme: Theme) {
    this._currentTheme.set(theme);
  }

  private updateOverlayClass(theme: Theme) {
    const classList = this.overlayContainer.getContainerElement().classList;
    classList.remove('dark-theme', 'light-theme');
    classList.add(`${theme}-theme`);
  }
}
