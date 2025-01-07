import { Component, Input } from '@angular/core';

@Component({
  selector: 'shared-field',
  standalone: false,
  templateUrl: './field.component.html',
  styleUrl: './field.component.sass'
})
export class FieldComponent {
  @Input() loading = false;
  @Input() field = '';
}
