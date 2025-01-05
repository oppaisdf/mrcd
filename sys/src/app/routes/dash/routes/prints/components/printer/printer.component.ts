import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';

@Component({
  selector: 'prints-printer',
  standalone: false,
  templateUrl: './printer.component.html',
  styleUrl: './printer.component.sass'
})
export class PrinterComponent {
  @ViewChild('pageA4') page!: ElementRef;
  @Input() fileName = 'defaultName';

  SaveToPDF() {
    const _page = this.page.nativeElement;

    html2canvas(_page, { scale: 2 }).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'mm', 'a4');

      const pdfWidth = pdf.internal.pageSize.getWidth();
      const pdfHeight = (canvas.height * pdfWidth) / canvas.width;

      pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
      pdf.save(`${this.fileName}.pdf`);
    });
  }
}
