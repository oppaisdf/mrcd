import { Component, input, signal } from '@angular/core';

let nextId = 0;
@Component({
  selector: 'ui-accordeon',
  imports: [],
  templateUrl: './accordeon.component.html',
  styleUrl: './accordeon.component.scss',
})
export class AccordeonComponent {
  title = input.required<string>();
  id = `ui-accordeon-${++nextId}`;
  readonly open = signal<boolean>(false);

  onToggle(ev: Event) {
    const details = ev.target as HTMLDetailsElement;
    this.open.set(details.open);
  }
}
