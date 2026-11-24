import { Component, computed, input, output } from '@angular/core';

export interface UiStickerItem {
  id: string;
  name: string;
  selected: boolean;
}

/** Presentation only: the consumer decides how and when to update selection. */
@Component({
  selector: 'ui-stickers',
  imports: [],
  templateUrl: './ui-stickers.component.html',
  styleUrl: './ui-stickers.component.scss',
})
export class UiStickersComponent {
  readonly label = input.required<string>();
  readonly items = input.required<ReadonlyArray<UiStickerItem>>();
  readonly disabled = input(false);
  readonly description = input('Elige una pegatina para seleccionarla o quitarla.');
  readonly selectedLabel = input('Seleccionado · Quitar');
  readonly unselectedLabel = input('Disponible · Seleccionar');
  readonly itemToggle = output<string>();
  readonly selectedCount = computed(() => this.items().filter(item => item.selected).length);

  toggle(id: string): void {
    if (!this.disabled()) this.itemToggle.emit(id);
  }
}
