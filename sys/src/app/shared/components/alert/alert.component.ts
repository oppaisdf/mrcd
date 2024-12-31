import { Component, Input } from '@angular/core';

@Component({
  selector: 'shared-alert',
  standalone: false,

  templateUrl: './alert.component.html',
  styleUrl: './alert.component.sass'
})
export class AlertComponent {
  @Input() message = '';
  @Input() success = false;
}
