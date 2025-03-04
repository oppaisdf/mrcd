import { Component, OnInit } from '@angular/core';
import { DegreeService } from '../../services/degree.service';
import { DefaultResponse } from '../../../../models/Response';

@Component({
  selector: 'degrees-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: DegreeService
  ) { }

  degrees: DefaultResponse[] = [];
  showUpdater = false;
  selectedDegree: DefaultResponse = {
    id: 0,
    name: ''
  };

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (response.success) this.degrees = response.data!;
  }

  Update(
    degree: DefaultResponse
  ) {
    this.selectedDegree = degree;
    this.showUpdater = true;
  }
}
