import { Component, HostBinding, inject, input, OnDestroy } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { UiSelectComponent } from '../select/ui-select.component';
import { SelectItem } from '../select/SelectItem';

@Component({
  selector: 'ui-print',
  imports: [
    ReactiveFormsModule,
    UiSelectComponent
  ],
  templateUrl: './ui-print.component.html',
  styleUrl: './ui-print.component.scss',
  host: {
    '[attr.data-orientation]': 'orientation'
  }
})
export class UiPrintComponent implements OnDestroy {
  private readonly _form = inject(FormBuilder);
  private pageStyleEl?: HTMLStyleElement;
  readonly form = this._form.nonNullable.group({
    orientation: ['portrait']
  });
  @HostBinding('attr.data-print-root') printRoot = 'true';
  title = input<string>();
  readonly orientations: Array<SelectItem<string>> = [{
    label: 'Vertical',
    value: 'portrait'
  }, {
    label: 'Horizontal',
    value: 'landscape'
  }];

  constructor() {
    window.addEventListener('afterprint', this.afterPrint);
  }

  ngOnDestroy() {
    window.removeEventListener('afterprint', this.afterPrint);
  }

  private afterPrint = () => {
    document.documentElement.classList.remove('print-mode');
    this.pageStyleEl?.remove();
    this.pageStyleEl = undefined;
  };

  print() {
    document.documentElement.classList.add('print-mode');

    const isLandscape = this.orientation === 'landscape';
    const size = isLandscape ? 'A4 landscape' : 'A4 portrait';

    this.pageStyleEl?.remove();
    this.pageStyleEl = document.createElement('style');
    this.pageStyleEl.setAttribute('data-ui-print-page', 'true');
    this.pageStyleEl.textContent = `
      @media print {
        @page { size: ${size}; margin: 12mm; }
      }
    `;
    document.head.appendChild(this.pageStyleEl);
    requestAnimationFrame(() => window.print());
  }

  get orientation() { return this.form.controls.orientation.value; }
}
