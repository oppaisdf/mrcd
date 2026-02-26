import { Component, HostBinding, inject, input } from '@angular/core';
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
export class UiPrintComponent {
  private readonly _form = inject(FormBuilder);
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
    window.addEventListener('afterprint', () => {
      document.documentElement.classList.remove('print-mode');
    });
  }

  print() {
    document.documentElement.classList.add('print-mode');
    requestAnimationFrame(() => window.print());
  }

  get orientation() { return this.form.controls.orientation.value; }
}
