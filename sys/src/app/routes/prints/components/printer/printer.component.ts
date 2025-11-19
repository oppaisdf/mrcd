import { Component, ElementRef, Input, NgZone, ViewChild } from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

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

  private async CreatePDF() {
    const root = this.page.nativeElement;
    console.log('Iniciando creación de PDF...');

    const pdf = new jsPDF({
      unit: 'mm',
      format: 'a4',
      orientation: this.isVertical ? 'portrait' : 'landscape'
    });

    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();

    const MAX_PAGE_HEIGHT_PX = this.isVertical ? 1122 : 794; // ajusta según tu layout real

    let hiddenContainer: HTMLElement | null = null;

    try {
      const pages = this.buildVirtualPages(root, MAX_PAGE_HEIGHT_PX);
      console.log('Páginas calculadas: ', pages.length);
      hiddenContainer = pages[0].parentElement as HTMLElement;

      for (let i = 0; i < pages.length; i++) {
        const pageEl = pages[i];

        const canvas = await html2canvas(pageEl, {
          scale: 1,
          useCORS: true,
          logging: false,
          windowWidth: pageEl.scrollWidth,
          windowHeight: pageEl.scrollHeight
        });

        const imgData = canvas.toDataURL('image/jpeg', 0.8);

        if (i > 0) {
          pdf.addPage();
        }

        pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);

        // dejar respirar al navegador
        await new Promise<void>(resolve => setTimeout(resolve, 0));
        console.log('Página ', i, ' agregada');
      }

      pdf.save(`${this.fileName}.pdf`);
    } catch (err) {
      console.error('Error generando PDF', err);
    } finally {
      if (hiddenContainer && hiddenContainer.parentElement) {
        hiddenContainer.parentElement.removeChild(hiddenContainer);
      }

      this._ngZone.run(() => {
        this.isCreating = false;
      });
    }
    console.log('Creación finalizada :3');
  }

  private buildVirtualPages(root: HTMLElement, maxHeight: number): HTMLElement[] {
    const pages: HTMLElement[] = [];

    const hiddenContainer = document.createElement('div');
    hiddenContainer.style.position = 'fixed';
    hiddenContainer.style.left = '-99999px';
    hiddenContainer.style.top = '0';
    hiddenContainer.style.width = root.offsetWidth + 'px';
    hiddenContainer.style.zIndex = '-1';
    document.body.appendChild(hiddenContainer);

    let currentPage = document.createElement('div');
    currentPage.style.width = '100%';
    hiddenContainer.appendChild(currentPage);
    pages.push(currentPage);

    const children = Array.from(root.children) as HTMLElement[];

    for (const child of children) {
      const clone = child.cloneNode(true) as HTMLElement;
      currentPage.appendChild(clone);

      if (currentPage.scrollHeight > maxHeight) {
        currentPage.removeChild(clone);

        currentPage = document.createElement('div');
        currentPage.style.width = '100%';
        hiddenContainer.appendChild(currentPage);
        pages.push(currentPage);

        currentPage.appendChild(clone);
      }
    }

    return pages;
  }
}