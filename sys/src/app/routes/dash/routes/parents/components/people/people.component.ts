import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BasicPersonResponse } from '../../../charges/models/responses/charge';
import { ParentService } from '../../services/parent.service';

@Component({
  selector: 'parent-c-people',
  standalone: false,
  templateUrl: './people.component.html',
  styleUrl: './people.component.sass'
})
export class PeopleComponent {
  constructor(
    private _service: ParentService
  ) { }

  @Input() updating = false;
  @Output() updatingChange = new EventEmitter<boolean>();
  @Input() people: BasicPersonResponse[] = [];
  @Input() id = 0;
  private _people: BasicPersonResponse[] = [];

  gender = '1';
  day = '1';
  name = '';
  message = '';
  success = true;

  ClearFilters() {
    this.gender = '1';
    this.day = '1';
    this.name = '';
    if (this._people.length === 0)
      this.people = this._people;
  }

  Filter() {
    if (this.gender === '1' && this.name.trim() === '' && this.day === '1') {
      this.ClearFilters();
      return;
    }
    if (this._people.length === 0)
      this._people = this.people;
    this.people = this._people.reduce((lst, p) => {
      switch (true) {
        case (this.gender === '1' && this.day === '1'):
          if (p.name.includes(this.name)) lst.push(p);
          break;
        case (this.gender === '1' && this.name.trim() === ''):
          if (p.day === (this.day === '2')) lst.push(p);
          break;
        case (this.name.trim() === '' && this.day === '1'):
          if (p.gender === (this.gender === '2')) lst.push(p);
          break;
        case (this.gender === '1'):
          if (p.day === (this.day === '2' && p.name.includes(this.name.trim()))) lst.push(p);
          break;
        case (this.day === '1'):
          if (p.gender === (this.gender === '2') && p.name.includes(this.name.trim())) lst.push(p);
          break;
        case (this.name.trim() === ''):
          if (p.gender === (this.gender === '2' && p.day === (this.day === '2'))) lst.push(p);
          break;
      }
      return lst;
    }, [] as BasicPersonResponse[]);
  }

  async SelectAsync(
    person: BasicPersonResponse,
    isParent: boolean
  ) {
    if (this.updating) return;
    this.updating = true;
    this.updatingChange.emit(true);

    const response = person.hasParent === undefined ?
      await this._service.AssignAsync(this.id, person.id, isParent) :
      await this._service.UnassignAsync(person.id, this.id);
    this.message = response.message;
    this.success = response.success;

    this.updating = false;
    this.updatingChange.emit(false);
    if (response.success) person.hasParent = isParent;
    else person.hasParent = undefined;
  }
}
