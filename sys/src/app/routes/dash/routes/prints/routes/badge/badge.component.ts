import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { QRResponse } from '../../responses/qr';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';

@Component({
  selector: 'prints-badge',
  standalone: false,
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.sass'
})
export class BadgeComponent implements OnInit {
  constructor(
    private _service: PrinterService
  ) { }

  qrs: QRResponse[] = [];
  @ViewChild('pageA4') page!: ElementRef;

  async ngOnInit() {
    const response = await this._service.GetQRsAsync();
    if (response.success) this.qrs = response.data!;
  }

  GeneratePDF() {
    const _page = this.page.nativeElement;

    html2canvas(_page, { scale: 2 }).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'mm', 'a4');

      const pdfWidth = pdf.internal.pageSize.getWidth();
      const pdfHeight = (canvas.height * pdfWidth) / canvas.width;

      pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
      pdf.save('gafetes.pdf');
    });
  }
}
