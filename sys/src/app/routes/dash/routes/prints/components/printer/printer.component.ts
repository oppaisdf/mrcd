import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import html2pdf from 'html2pdf.js';

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

  isCreating = false;

  SaveToPDF() {
    if (this.isCreating) return;
    this.isCreating = true;
    const pageElement = this.page.nativeElement;

    const opt = {
      margin: 0,
      filename: `${this.fileName}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'mm', format: 'a4', orientation: (this.isVertical ? 'portrait' : 'landscape') }
    };

    html2pdf()
      .from(pageElement)
      .set(opt)
      .save();
    this.isCreating = false;
  }
}