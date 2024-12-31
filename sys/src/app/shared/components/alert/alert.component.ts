import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'shared-alert',
  standalone: false,

  templateUrl: './alert.component.html',
  styleUrl: './alert.component.sass'
})
export class AlertComponent implements OnChanges {
  @Input() message = '';
  @Input() success = false;

  ngOnChanges(
    changes: SimpleChanges
  ) {
    if (!changes['message'] || !this.message) return;
    setTimeout(() => {
      this.message = '';
    }, 5000);
  }
}
