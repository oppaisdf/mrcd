import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarService } from '../../services/calendar.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivityStageResponse } from '../../models/responses';
import { DefaultResponse } from '../../../../core/models/responses/Response';
import { SimpleUserResponse } from '../../../users/models/responses/user';
import { LoginService } from '../../../../services/login.service';
import { ActivityStageRequest } from '../../models/requests';

@Component({
  selector: 'planner-activity',
  standalone: false,
  templateUrl: './activity.html',
  styleUrl: './activity.sass'
})
export class Activity implements OnInit {
  constructor(
    private _me: ActivatedRoute,
    private _service: CalendarService,
    private _form: FormBuilder,
    private _login: LoginService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: [''],
      date: ['']
    });
    this.newStage = this._form.group({
      stageId: [0],
      mainUser: ['false'],
      userId: [''],
      notes: ['']
    });
  }

  private _id: number = 0;
  message = '';
  success = false;
  updating = false;
  form: FormGroup;
  newStage: FormGroup;
  activities: ActivityStageResponse[] = [];
  stages: DefaultResponse[] = [];
  users: SimpleUserResponse[] = [];
  isAdm = false;

  async ngOnInit() {
    this.isAdm = this._login.HasUserPermission('adm');
    this._id = +`${this._me.snapshot.paramMap.get('id')}`;
    this.form.disable();
    await this.LoadActivity();
  }

  private async LoadActivity() {
    this.updating = true;
    const response = await this._service.ActivityById(this._id);
    this.message = response.message;
    this.success = response.success;
    this.updating = false;
    if (!response.success) return;
    this.form.patchValue({
      name: response.data!.activity.name,
      date: response.data!.activity.date
    });
    this.activities = response.data!.activity.activities;
    this.stages = response.data!.stages;
    this.users = response.data!.users;
    this.users.push({
      id: '',
      name: ''
    });
  }

  async DeleteAsync() {
    if (this.updating) return;
    this.updating = true;

    const response = await this._service.DeleteActivityAsync(this._id);
    this.message = response.message;
    this.success = response.success;
    this.updating = false;

    if (!this.success) return;
    this._router.navigateByUrl('/planner');
  }

  private GetValue(
    name: string
  ) {
    return `${this.newStage.controls[name].value}`.trim().replace('null', '');
  }

  async AddStageToActivityAsync() {
    if (this.updating) return;
    this.updating = true;

    const request: ActivityStageRequest = {
      activityId: this._id,
      stageId: +this.GetValue('stageId'),
      mainUser: this.GetValue('mainUser') === 'true' && this.GetValue('userId') !== '',
      userId: this.GetValue('userId') === '' ? undefined : this.GetValue('userId'),
      notes: this.GetValue('notes') === '' ? undefined : this.GetValue('notes')
    };
    const response = await this._service.AddActivityToStageAsync(request);
    this.message = response.message;
    this.success = response.success;
    this.updating = false;

    if (!this.success) return;
    this.newStage.reset();
    this.newStage.patchValue({
      mainUser: 'false'
    });
    await this.LoadActivity();
  }

  async DeleteStageAsync(
    id: number
  ) {
    if (this.updating) return;
    this.updating = true;

    const response = await this._service.DelStageToActivityAsync(this._id, id);
    this.message = response.message;
    this.success = response.success;
    this.updating = false;

    if (!response.success) return;
    await this.LoadActivity();
  }
}
