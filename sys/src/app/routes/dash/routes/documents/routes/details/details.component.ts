import { Component, OnDestroy, OnInit } from '@angular/core';
import { DocService } from '../../services/doc.service';
import { ActivatedRoute } from '@angular/router';
import { BasicPersonResponse } from '../../../charges/models/responses/charge';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'documents-details',
  standalone: false,
  templateUrl: './details.component.html',
  styleUrl: './details.component.sass'
})
export class DetailsComponent implements OnInit, OnDestroy {
  constructor(
    private _service: DocService,
    private _me: ActivatedRoute,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: [''],
      day: [],
      gender: []
    });
  }

  message = '';
  success = true;
  isLoading = false;
  title = '';
  private _people: BasicPersonResponse[] = [];
  people: BasicPersonResponse[] = [];
  form: FormGroup;
  private _subscriptions = new Subscription();
  private _id = 0;

  async ngOnInit() {
    this._id = +this._me.snapshot.paramMap.get('id')!;
    const response = await this._service.GetByIdAsync(this._id);
    this.message = response.message;
    this.success = response.success;
    if (!response.success) return;
    this.title = response.data!.name;
    this._people = response.data!.people.sort((x, y) => x.name.localeCompare(y.name));
    this.people = this._people;
    this._subscriptions.add(
      this.form.valueChanges.subscribe(() => this.Filter())
    );
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  private GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`;
  }

  private Filter() {
    const name = this.GetValue('name');
    const day = (() => {
      switch (this.GetValue('day')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    const gender = (() => {
      switch (this.GetValue('gender')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    this.people = this._people.filter(q =>
      (name === '' || q.name.includes(name)) &&
      (day === undefined || q.day === day) &&
      (gender === undefined || q.gender === gender)
    );
  }

  ClearFilters() {
    this.form.reset();
    this.people = this._people
  }

  async AssingAsync(
    person: BasicPersonResponse
  ) {
    if (this.isLoading) return;
    this.isLoading = true;
    const response = person.isActive ?
      await this._service.UnassignAsync(this._id, person.id) :
      await this._service.AssignAsync(this._id, person.id);
    this.message = response.message;
    this.success = response.success;
    this.isLoading = false;
    if (!response.success) return;
    person.isActive = !person.isActive;
  }
}
