import { Component } from '@angular/core';
import { DefaultResponse } from '../../../../core/models/responses/Response';
import { LoginService } from '../../../../services/login.service';
import { StageService } from '../../services/stage';

@Component({
  selector: 'app-stage',
  standalone: false,
  templateUrl: './stage.html',
  styleUrl: './stage.sass'
})
export class Stage {
  constructor(
    private _service: StageService,
    private _login: LoginService
  ) { }

  stages: DefaultResponse[] = [];
  isAdm = false;
  message = '';
  success = false;
  loading = false;
  showModal = false;

  async ngOnInit() {
    this.isAdm = this._login.HasUserPermission('adm');
    await this.LoadStages();
  }

  private async LoadStages() {
    this.loading = true;
    const response = await this._service.StagesToListAsync();
    this.loading = false;
    if (!response.success) return;
    this.stages = response.data!;
  }

  async DeleteAsync(
    id: number
  ) {
    if (this.loading) return;
    this.loading = true;

    const response = await this._service.DeleteStageAsync(id);
    this.message = response.message;
    this.success = response.success;
    this.loading = false;

    if (!response.success) return;
    await this.LoadStages();
  }

  NewStage() {
    if (this.loading) return;
    this.showModal = true;
  }
}
