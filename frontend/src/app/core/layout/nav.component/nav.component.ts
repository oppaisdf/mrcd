import { Component, computed, DestroyRef, EventEmitter, HostListener, inject, Output, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CategoryId, NAV_CATEGORIES } from './nav.config';
import { SessionStore } from '../../stores/session.store';
import { AuthService } from '../../auth/auth.service';
import { ThemeService } from '../../ui/theme/theme.service';

@Component({
  selector: 'app-nav',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss',
})
export class NavComponent {
  @Output() toggleTheme = new EventEmitter<void>();
  private readonly session = inject(SessionStore);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly theme = inject(ThemeService).theme;
  readonly selectedId = signal<CategoryId | undefined>(undefined);
  readonly mobileOpen = signal(false);
  readonly desktopOpen = signal(false);
  readonly currentUrl = signal(this.router.url);
  readonly categories = computed(() => {
    const roles = this.session.roles();
    return NAV_CATEGORIES.map(category => ({
      ...category,
      options: category.options.filter(option => option.roles.some(role => roles.includes(role)))
    })).filter(category => category.roles.some(role => roles.includes(role)) && category.options.length > 0);
  });
  readonly selectedCategory = computed(() => this.categories().find(c => c.id === this.selectedId()) ?? this.categories()[0]);
  readonly activeCategory = computed(() => this.categories().find(c => c.options.some(o => this.currentUrl().split('?')[0] === o.route))?.id);
  readonly icons: Record<string, string> = {
    Confirmandos: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2 M16 3a4 4 0 0 1 0 8 M22 21v-2a4 4 0 0 0-3-3.87 M13 7a4 4 0 1 1-8 0 4 4 0 0 1 8 0',
    Impresiones: 'M6 9V3h12v6 M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2 M6 14h12v8H6z M18 12h.01',
    Gestiones: 'M3 3h7v7H3z M14 3h7v7h-7z M3 14h7v7H3z M14 14h7v7h-7z',
    Monitoreo: 'M3 3v18h18 M7 14l4-4 4 3 6-8',
    System: 'M12 8a4 4 0 1 0 0 8 4 4 0 0 0 0-8 M12 2v3 M12 19v3 M2 12h3 M19 12h3 M5 5l2 2 M17 17l2 2 M5 19l2-2 M17 7l2-2'
  };

  constructor() {
    this.router.events.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentUrl.set(event.urlAfterRedirects);
        this.closeDrawer();
      }
    });
  }

  selectCategory(id: CategoryId): void {
    const same = this.selectedCategory()?.id === id;
    this.selectedId.set(id);
    this.desktopOpen.set(same ? !this.desktopOpen() : true);
  }

  toggleMobile(): void {
    this.desktopOpen.set(false);
    this.mobileOpen.update(open => !open);
    if (this.mobileOpen()) this.selectedId.set(this.activeCategory() ?? this.categories()[0]?.id);
  }

  @HostListener('document:keydown.escape')
  closeDrawer(): void {
    this.desktopOpen.set(false);
    this.mobileOpen.set(false);
  }

  @HostListener('window:resize')
  onResize(): void { this.closeDrawer(); }

  logout(): void {
    this.closeDrawer();
    this.auth.logout();
  }
}
