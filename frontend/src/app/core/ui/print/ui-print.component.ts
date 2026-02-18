import { Component, HostBinding, input } from '@angular/core';
import { Paper } from './paper';
import { Orientation } from './orientation';

@Component({
  selector: 'ui-print',
  imports: [],
  templateUrl: './ui-print.component.html',
  styleUrl: './ui-print.component.scss',
})
export class UiPrintComponent {
  @HostBinding('attr.data-print-root') printRoot = 'true';
  paper = input.required<Paper>();
  orientation = input.required<Orientation>();
  title = input<string>();

  constructor() {
    window.addEventListener('afterprint', () => {
      document.documentElement.classList.remove('print-mode');
    });
  }

  print() {
    document.documentElement.classList.add('print-mode');
    requestAnimationFrame(() => window.print());
  }
}
