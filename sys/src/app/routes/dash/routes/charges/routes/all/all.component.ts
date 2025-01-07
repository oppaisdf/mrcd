import { Component, OnInit } from '@angular/core';
import { ChargeService } from '../../services/charge.service';
import { ChargeResponse } from '../../models/responses/charge';
import { environment } from '../../../../environments/environment';
import { LoginService } from '../../../../../../services/login.service';

@Component({
  selector: 'charge-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: ChargeService,
    private _login: LoginService
  ) { }

  charges: ChargeResponse[] = [];
  selectedCharge: ChargeResponse = {
    id: 0,
    name: '',
    total: 0,
    isActive: true
  };
  showUpdater = false;
  currencySymbol = environment.currencySymbol;
  isAdm = false;

  async ngOnInit() {
    this.isAdm = this._login.HasUserPermission('adm');
    const response = await this._service.GetAsync();
    if (response.success) this.charges = response.data!;
  }

  SelectCharge(
    charge: ChargeResponse
  ) {
    this.selectedCharge = charge;
    this.showUpdater = true;
  }
}
