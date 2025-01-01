import { Component, OnInit } from '@angular/core';
import { ActionService } from '../../services/action.service';
import { DefaultResponse } from '../../../../models/Response';

@Component({
  selector: 'app-all',
  standalone: false,

  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: ActionService
  ) { }

  isUpdating = false;
  success = true;
  showUpdater = false;
  message = '';
  actions: DefaultResponse[] = [];
  selectedAction: DefaultResponse = {
    id: 0,
    name: ''
  };

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (response.success) this.actions = response.data!;
    else {
      this.message = response.message;
      this.success = response.success;
    }
  }

  SelectAction(
    action: DefaultResponse
  ) {
    this.selectedAction = action;
    this.showUpdater = true;
  }
}
