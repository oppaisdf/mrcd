import { Component, Input } from '@angular/core';

@Component({
  selector: 'shared-pan-expand',
  standalone: false,
  templateUrl: './pan-expand.component.html',
  styleUrl: './pan-expand.component.sass'
})
export class PanExpandComponent {
  @Input() title: string = '';
  isExpanded = false;
  icon = 'v';

  Expand() {
    this.isExpanded = !this.isExpanded;
    this.icon = this.isExpanded ? '^' : 'v';
  }
}
