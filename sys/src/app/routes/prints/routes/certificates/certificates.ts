import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GeneralParentListResponse, ListGeneralResponse } from '../../responses/list';
import { PrinterService } from '../../services/printer.service';

@Component({
  selector: 'app-certificates',
  standalone: false,
  templateUrl: './certificates.html',
  styleUrl: './certificates.sass'
})
export class Certificates implements OnInit {
  constructor(
    private _form: FormBuilder,
    private _service: PrinterService
  ) {
    this.form = this._form.group({
      isVertical: ['true'],
      date: [''],
      church: [''],
      signature: [''],
      cite: [''],
      officiant: ['']
    });
  }

  form: FormGroup;
  people: ListGeneralResponse[] = [];

  GetValue(key: string) {
    return `${this.form.controls[key].value}`;
  }

  GetParents(
    parents: GeneralParentListResponse[] | undefined
  ) {
    if (!parents) return '';
    return parents.reduce((a, b) => {
      return a += `${a.length > 0 ? ' y ' : ''}${b.name}`;
    }, '');
  }

  GetDOB(
    date: Date
  ) {
    const dob = new Date(date);
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }

  async ngOnInit() {
    const response = await this._service.GetGeneralList();
    if (!response.success) return;
    this.people = response.data!;
  }
}
