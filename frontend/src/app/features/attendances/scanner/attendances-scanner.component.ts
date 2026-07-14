import { Component, ElementRef, OnDestroy, output, signal, ViewChild } from '@angular/core';
import { BrowserMultiFormatReader, IScannerControls } from '@zxing/browser';
import {
  BarcodeFormat,
  DecodeHintType,
  ChecksumException,
  FormatException,
  NotFoundException,
} from '@zxing/library';

@Component({
  selector: 'attendances-scanner',
  imports: [],
  templateUrl: './attendances-scanner.component.html',
  styleUrl: './attendances-scanner.component.scss',
})
export class AttendancesScannerComponent implements OnDestroy {
  @ViewChild('video', { static: true }) private video!: ElementRef<HTMLVideoElement>;
  goScan = output<string>();
  private readonly _reader: BrowserMultiFormatReader;
  private _controls: IScannerControls | null = null;

  constructor() {
    const hints = new Map();
    hints.set(DecodeHintType.POSSIBLE_FORMATS, [BarcodeFormat.QR_CODE]);
    this._reader = new BrowserMultiFormatReader(hints);
  }

  stop() {
    this._controls?.stop();
    this._controls = null;
  }

  pause() { this._controls?.stop(); }

  ngOnDestroy() {
    this.stop();
  }

  async startScannAsync() {
    this._controls = await this._reader.decodeFromVideoDevice(
      undefined,
      this.video.nativeElement,
      (result, error) => {
        if (result) {
          this.goScan.emit(result.getText());
          this.stop();
          return;
        }

        if (
          error instanceof NotFoundException ||
          error instanceof ChecksumException ||
          error instanceof FormatException
        ) return;

        if (error)
          console.error('[+] Error inesperado:', error);
      }
    );
  }
}
