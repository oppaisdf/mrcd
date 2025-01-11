import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ChargeService } from '../../services/charge.service';
import { ChargeResponse } from '../../../charges/models/responses/charge';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'people-comp-charges',
  standalone: false,
  templateUrl: './charges.component.html',
  styleUrl: './charges.component.sass'
})
export class ChargesComponent {
  constructor(
    private _service: ChargeService
  ) { }

  @Input() id = 0;
  @Input() charges: ChargeResponse[] = [];
  @Input() updating = false;
  @Output() updatingChange = new EventEmitter<boolean>();

  message = '';
  success = true;
  currencySymbol = environment.currencySymbol;

  async AssignAsync(
    charge: ChargeResponse
  ) {
    if (this.updating) return;
    this.updating = true;
    this.updatingChange.emit(this.updating);

    const response = !charge.isActive ?
      await this._service.AssignAsync(charge.id, this.id) :
      await this._service.UnassingAsync(charge.id, this.id);
    this.message = response.message;
    this.success = response.success;

    this.updating = false;
    this.updatingChange.emit(this.updating);
    if (response.success) charge.isActive = !charge.isActive;
  }
}
