import { Component, computed, EventEmitter, inject, Output, signal } from '@angular/core';
import { CategoryId, CategoryMenu, NAV_CATEGORIES } from './nav.config';
import { SessionStore } from '../../stores/session.store';
import { AuthService } from '../../auth/auth.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-nav',
  imports: [RouterLink],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss',
})
export class NavComponent {
  @Output() toggleTheme = new EventEmitter<void>();
  private readonly _categories = signal<CategoryMenu[]>(NAV_CATEGORIES);
  private readonly _session = inject(SessionStore);
  private readonly _service = inject(AuthService);
  private readonly _route = inject(Router);

  readonly selectedCategory = signal<CategoryMenu | undefined>(undefined);
  readonly categories = computed(() => {
    const roles = this._session.roles();
    return this._categories()
      .map(category => ({
        ...category,
        options: category.options.filter(option => option.roles.some(role => roles.includes(role)))
      }))
      .filter(category => category.roles.some(role => roles.includes(role)) && category.options.length > 0);
  });

  selectCategory(
    categoryId: CategoryId | undefined,
    ev?: Event
  ) {
    ev?.preventDefault();

    if (categoryId === 'home') {
      this.selectedCategory.set(undefined);
      this._route.navigateByUrl('/');
      return;
    }

    const category = this.categories()
      .find(c => c.id == categoryId);
    this.selectedCategory.set(category);
  }

  logout() {
    this._service.logout();
  }

  protected readonly isDrawerOpen = computed(() => {
    const c = this.selectedCategory();
    return !!c && c.id !== 'home';
  });

  protected closeDrawer() {
    this.selectedCategory.set(undefined);
  }
}
