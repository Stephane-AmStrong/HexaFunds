import { Injectable, computed, effect, signal } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private mediaQueryList: MediaQueryList;

  private readonly _currentTheme = signal<'light' | 'dark'>('light');

  constructor() { 
    this.mediaQueryList = window.matchMedia('(prefers-color-scheme: dark)');
    this._currentTheme.set(this.mediaQueryList.matches ? 'dark' : 'light');
    this.mediaQueryList.addEventListener('change', this.handleMediaChange);
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  private handleMediaChange = (event: MediaQueryListEvent): void => {
    this._currentTheme.set(event.matches? 'dark': 'light');
  }

  getTheme() {
    return computed(() => this._currentTheme());
  }
}
