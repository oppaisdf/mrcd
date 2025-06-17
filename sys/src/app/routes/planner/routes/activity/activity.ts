import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CalendarService } from '../../services/calendar.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivityStageResponse } from '../../models/responses';
import { DefaultResponse } from '../../../../core/models/responses/Response';
import { SimpleUserResponse } from '../../../users/models/responses/user';
import { LoginService } from '../../../../services/login.service';

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
    private _login: LoginService
  ) {
    this.form = this._form.group({
      name: [''],
      date: ['']
    });
    this.newStage = this._form.group({
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
}
