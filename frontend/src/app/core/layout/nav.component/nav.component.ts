import { Component, computed, EventEmitter, inject, Output, signal } from '@angular/core';
import { ViewTransitionService } from '../../ui/transitions/view-transitions.service';
import { Router, RouterLinkWithHref } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { SessionStore } from '../../stores/session.store';
import { CategoryMenu, NAV_CATEGORIES } from './nav.config';

@Component({
  selector: 'app-nav',
  imports: [RouterLinkWithHref],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss',
})
export class NavComponent {
  @Output() toggleTheme = new EventEmitter<void>();

  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly session = inject(SessionStore);
  protected vt = inject(ViewTransitionService);

  protected isAuth = this.session.isAuthenticated;

  // estado UI del flyout
  protected isFlyoutOpen = signal(false);
  protected activeCategoryId = signal<string | null>(null);

  protected categories = NAV_CATEGORIES;

  protected visibleCategories = computed(() => {
    const roles = 'sys'; //this.session.roles();
    return this.categories.filter(c =>
      !c.roles?.length || c.roles.some(r => roles.includes(r as any))
    );
  });

  protected activeCategory = computed(() => {
    const id = this.activeCategoryId();
    return this.visibleCategories().find(c => c.id === id) ?? null;
  });

  protected visibleItems = computed(() => {
    const cat = this.activeCategory();
    if (!cat) return [];
    const roles = ['sys']; //this.session.roles();
    return cat.options.filter(i =>
      !i.roles?.length || i.roles.some(r => roles.includes(r as any))
    );
  });

  toggleCategory(id: string) {
    if (this.activeCategoryId() === id) {
      this.isFlyoutOpen.set(!this.isFlyoutOpen());
      return;
    }
    this.activeCategoryId.set(id);
    this.isFlyoutOpen.set(true);
  }

  onCategoryClick(cat: CategoryMenu) {
    if (this.activeCategoryId() === cat.id && this.isFlyoutOpen())
      this.closeFlyout();
    else {
      this.activeCategoryId.set(cat.id);
      this.isFlyoutOpen.set(true);
    }
  }

  closeFlyout() {
    this.isFlyoutOpen.set(false);
  }

  onLeafClick(ev: Event, route: string) {
    // aquí SÍ: dispara animación (más adelante)
    // por ahora dejamos navegación normal, y luego cambias a vt.go(route)
    ev.preventDefault();
    this.closeFlyout();
    void this.router.navigateByUrl(route);
    // más adelante: void this.vt.go(route);
  }

  async logout() {
    this.auth.logout();
    this.closeFlyout();
    await this.router.navigateByUrl('/');
  }
}
