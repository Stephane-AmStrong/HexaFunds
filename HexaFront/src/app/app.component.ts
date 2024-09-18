import { Component, HostBinding, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './shared/services/theme.service';
import { LayoutComponent } from "./shared/Components/layout/layout.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'HexaFront';

  themeService = inject(ThemeService)
  
  private isDark = this.themeService.getTheme()() === 'dark';

  @HostBinding('class')
  get themeMode(){
    return this.isDark ? 'dark-theme' : 'light-theme'
  }

  toggleTheme(isDarkMode: boolean) {
    this.isDark = isDarkMode;
  }
}
