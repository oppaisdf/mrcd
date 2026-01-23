import { Component, computed, EventEmitter, inject, Output, signal } from '@angular/core';
import { CategoryId, CategoryMenu, NAV_CATEGORIES } from './nav.config';
import { ViewTransitionService } from '../../ui/transitions/view-transitions.service';

@Component({
  selector: 'app-nav',
  imports: [],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss',
})
export class NavComponent {
  @Output() toggleTheme = new EventEmitter<void>();
  private readonly _categories = signal<CategoryMenu[]>(NAV_CATEGORIES);

  readonly router = inject(ViewTransitionService);
  readonly selectedCategory = signal<CategoryMenu | undefined>(undefined);
  readonly categories = computed(() => {
    const roles = ['sys', 'usr', 'adm'];//this._session.roles();
    return this._categories()
      .filter(category =>
        category.roles.some(role => roles.includes(role))
      );
  });

  isActiveCategory(
    categoryId: CategoryId
  ) {
    const currentCategory = this.selectedCategory();
    return currentCategory?.id == categoryId;
  }

  selectCategory(
    categoryId: CategoryId | undefined
  ) {
    const category = this.categories()
      .find(c => c.id == categoryId);
    this.selectedCategory.set(category);
  }
}
