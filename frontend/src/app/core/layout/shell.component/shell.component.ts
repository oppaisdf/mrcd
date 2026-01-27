import { Component, computed, inject } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { NavComponent } from "../nav.component/nav.component";
import { SessionStore } from '../../stores/session.store';
import { ThemeService } from '../../ui/theme/theme.service';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, NavComponent],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
})
export class ShellComponent {
  private readonly theme = inject(ThemeService);
  private readonly _session = inject(SessionStore);

  protected readonly year = new Date().getFullYear();
  protected readonly currentTheme = computed(() => this.theme.theme());
  protected readonly isAuth = this._session.isAuthenticated;

  protected toggleTheme(): void {
    this.theme.toggle();
  }
}
