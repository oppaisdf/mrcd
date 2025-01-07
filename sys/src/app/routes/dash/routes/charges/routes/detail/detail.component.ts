import { Component, OnInit } from '@angular/core';
import { ChargeService } from '../../services/charge.service';
import { PersonChargeResponse } from '../../models/responses/charge';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'charge-detail',
  standalone: false,
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.sass'
})
export class DetailComponent implements OnInit {
  constructor(
    private _service: ChargeService,
    private _me: ActivatedRoute
  ) { }

  private id = 0;
  name = '';
  total = 0;
  saturday: PersonChargeResponse[] = [];
  sunday: PersonChargeResponse[] = [];
  private _people: PersonChargeResponse[] = [];
  message = '';
  success = true;
  isUpdating = false;
  currencySymbol = environment.currencySymbol;
  gender = '1';
  txt = '';

  async ngOnInit() {
    const id = +`${this._me.snapshot.paramMap.get('id')}`;
    const response = await this._service.GetByIdAsync(id);
    this.message = response.message;
    this.success = response.success;
    if (!response.success) return;

    this.id = id;
    this.name = response.data!.name;
    this.total = response.data!.total;
    if (!response.data!.people) return;
    this._people = response.data!.people;
    this._people.sort((a, b) => a.name.localeCompare(b.name));
    this.ClearFilters();
  }

  async SelectPerson(
    person: PersonChargeResponse
  ) {
    if (this.isUpdating) return;
    this.isUpdating = true;

    const response = !person.isActive ?
      await this._service.AssignAsync(this.id, person.id) :
      await this._service.UnassingAsync(this.id, person.id);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    if (response.success) person.isActive = !person.isActive;
  }

  private ClearFilters() {
    this.gender = '1';
    this.txt = '';
    this.saturday = this._people.reduce((lst, p) => {
      if (!p.day) lst.push(p);
      return lst;
    }, [] as PersonChargeResponse[]);
    this.sunday = this._people.reduce((lst, p) => {
      if (p.day) lst.push(p);
      return lst;
    }, [] as PersonChargeResponse[]);
  }

  Filter() {
  }
}
