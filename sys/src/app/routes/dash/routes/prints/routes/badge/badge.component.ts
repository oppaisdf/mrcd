import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { QRResponse } from '../../responses/qr';

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

  async ngOnInit() {
    const response = await this._service.GetQRsAsync();
    if (response.success) this.qrs = response.data!;
  }
}
