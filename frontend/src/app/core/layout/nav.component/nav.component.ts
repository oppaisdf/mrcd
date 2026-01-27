import { Component, computed, EventEmitter, inject, Output, signal } from '@angular/core';
import { CategoryId, CategoryMenu, NAV_CATEGORIES } from './nav.config';
import { SessionStore } from '../../stores/session.store';
import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  imports: [],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss',
})
export class NavComponent {
  @Output() toggleTheme = new EventEmitter<void>();
  private readonly _categories = signal<CategoryMenu[]>(NAV_CATEGORIES);
  private readonly _session = inject(SessionStore);
  private readonly _service = inject(AuthService);

  readonly router = inject(Router);
  readonly selectedCategory = signal<CategoryMenu | undefined>(undefined);
  readonly categories = computed(() => {
    const roles = this._session.roles();
    return this._categories()
      .filter(category =>
        category.roles.some(role => roles.includes(role))
      );
  });

  selectCategory(
    categoryId: CategoryId | undefined,
    ev?: Event
  ) {
    ev?.preventDefault();
    const category = this.categories()
      .find(c => c.id == categoryId);
    this.selectedCategory.set(category);
  }

  logout() {
    this._service.logout();
  }

  protected go(
    route: string | any[],
    ev?: Event
  ) {
    ev?.preventDefault();
    this.router.navigate(Array.isArray(route) ? route : [route]);
  }

  protected readonly isDrawerOpen = computed(() => {
    const c = this.selectedCategory();
    return !!c && c.id !== 'home';
  });

  protected closeDrawer() {
    this.selectedCategory.set(undefined);
  }
}
