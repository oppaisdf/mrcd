import { Component, ElementRef, Input, NgZone, ViewChild } from '@angular/core';
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
  @Input() usePadding = true;

  constructor(
    private readonly _ngZone: NgZone
  ) { }

  isCreating = false;

  SaveToPDF() {
    if (this.isCreating) return;
    this.isCreating = true;
    setTimeout(() => {
      this.CreatePDF();
    }, 0);
  }

  private CreatePDF() {
    const pageElement = this.page.nativeElement;

    const opt = {
      margin: 0,
      filename: `${this.fileName}.pdf`,
      image: { type: 'jpeg', quality: 0.8 },
      html2canvas: { scale: 1 },
      jsPDF: {
        unit: 'mm',
        format: 'a4',
        orientation: (this.isVertical ? 'portrait' : 'landscape')
      }
    };

    html2pdf()
      .from(pageElement)
      .set(opt)
      .save()
      .thenExternal(() => {
        this._ngZone.run(() => {
          this.isCreating = false;
        });
      })
      .catchExternal((err: any) => {
        console.error('Error generando PDF', err);
        this._ngZone.run(() => {
          this.isCreating = false;
        });
      });
  }
}