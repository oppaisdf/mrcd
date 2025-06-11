import { Component, OnInit } from '@angular/core';
import { SacramentService } from '../../services/sacrament.service';
import { DefaultResponse } from '../../../../core/models/responses/Response';

@Component({
  selector: 'sacrament-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: SacramentService
  ) { }

  sacraments: DefaultResponse[] = [];
  showUpdater = false;
  message = '';
  success = true;
  selectedSacrament: DefaultResponse = {
    id: 0,
    name: ''
  };

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (response.success) this.sacraments = response.data!;
    this.message = response.message;
    this.success = response.success;
  }

  Select(
    sacrament: DefaultResponse
  ) {
    this.selectedSacrament = sacrament;
    this.showUpdater = true;
  }
}
