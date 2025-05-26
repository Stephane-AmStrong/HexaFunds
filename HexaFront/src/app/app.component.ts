import { Component, HostBinding, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './shared/services/theme.service';
import { LayoutComponent } from './shared/Components/layout/layout.component';
import { MatGridListModule } from '@angular/material/grid-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LayoutComponent, MatGridListModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'HexaFront';

  themeService = inject(ThemeService);

  @HostBinding('class')
  get themeMode() {
    return `${this.themeService.getTheme()()}-theme`;
  }

  toggleTheme(isDarkMode: boolean) {
    this.themeService.setTheme(isDarkMode ? 'dark' : 'light');
  }
}
