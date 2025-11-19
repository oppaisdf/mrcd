import { Component, ElementRef, Input, NgZone, ViewChild } from '@angular/core';

@Component({
  selector: 'prints-printer',
  standalone: false,
  templateUrl: './printer.component.html',
  styleUrl: './printer.component.sass'
})
export class PrinterComponent {
  @ViewChild('pageA4') page!: ElementRef;
  @Input() fileName = 'defaultName';
  @Input() isVertical = true;
  @Input() usePadding = true;

  constructor(
    private readonly _ngZone: NgZone
  ) { }

  isCreating = false;
  get MAX_PAGE_HEIGHT_PX() {
    return this.isVertical ? 1122 : 794;
  }

  print() {
    const originalTitle = document.title;
    const body = document.body;

    const orientationClass = this.isVertical ? 'print-portrait' : 'print-landscape';
    body.classList.add(orientationClass);

    if (this.fileName) {
      document.title = this.fileName;
    }

    window.print();

    // Quitar la clase tras imprimir (el diálogo de impresión bloquea el hilo)
    setTimeout(() => {
      body.classList.remove(orientationClass);
      document.title = originalTitle;
    }, 0);
  }
}