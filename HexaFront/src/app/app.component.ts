import { Component, HostBinding, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TemplateComponent } from './shared/Components/template/template.component';
import { ThemeService } from './shared/services/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TemplateComponent],
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
