import { Component, inject } from '@angular/core';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { ParentFormComponent } from "../form/parent-form.component";
import { CreateParentRequest } from '../requests/create-parent.request';
import { ParentService } from '../services/parent.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-parents.page',
  imports: [ParentFormComponent],
  templateUrl: './create-parents.page.html',
  styleUrl: './create-parents.page.scss',
})
export class CreateParentsPage {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(ParentService);
  private readonly _route = inject(Router);

  async createAsync(
    request: CreateParentRequest
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.createAsync(request);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success('Se ha agregado el padre/padrino correctamente');
    this._route.navigateByUrl("/parents");
  }
}
