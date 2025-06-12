import { Component, ElementRef, EventEmitter, OnDestroy, Output, ViewChild } from '@angular/core';
import { BrowserMultiFormatReader, IScannerControls } from '@zxing/browser';
import { BarcodeFormat, DecodeHintType } from '@zxing/library';


@Component({
  selector: 'attendance-comp-scanner',
  standalone: false,
  templateUrl: './scanner.component.html',
  styleUrl: './scanner.component.sass'
})
export class ScannerComponent implements OnDestroy {
  @ViewChild('video', { static: true }) private video!: ElementRef<HTMLVideoElement>;
  @Output() scanned = new EventEmitter<string>();

  private _reader: BrowserMultiFormatReader;
  private controls: IScannerControls | null = null;

  //result: string | null = null;

  constructor() {
    const hints = new Map();
    hints.set(DecodeHintType.POSSIBLE_FORMATS, [BarcodeFormat.QR_CODE]);
    this._reader = new BrowserMultiFormatReader(hints);
  }

  ngOnDestroy() {
    this.Stop();
  }

  public Pause() {
    this.controls?.stop();
  }

  async Start() {
    this.controls = await this._reader.decodeFromVideoDevice(
      undefined,
      this.video.nativeElement,
      (rst, err) => {
        if (rst) {
          this.scanned.emit(rst.getText());
          //this.result = rst.getText();
          this.Stop();
        }
        if (err && err.name !== 'NotFoundException' && err.name !== 'NotFoundException2')
          console.log('[+] Error inesperado: ', err);
      }
    );
  }

  private Stop() {
    this.controls?.stop();
    this.controls = null;
  }
}
