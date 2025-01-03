import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'shared-alert',
  standalone: false,
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.sass'
})
export class AlertComponent implements OnChanges {
  @Input() message = '';
  @Output() messageChange = new EventEmitter<string>();
  @Input() success = false;

  ngOnChanges(
    changes: SimpleChanges
  ) {
    if (!changes['message'] || !this.message) return;
    setTimeout(() => {
      this.messageChange.emit('');
    }, 5000);
  }

  Hide() {
    this.message = '';
  }
}
