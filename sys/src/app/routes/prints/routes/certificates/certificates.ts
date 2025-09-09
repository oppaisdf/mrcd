import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ListGeneralResponse } from '../../responses/list';
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
      date: ['']
    });
  }

  form: FormGroup;
  people: ListGeneralResponse[] = [];

  GetValue(key: string) {
    return `${this.form.controls[key].value}`;
  }

  async ngOnInit() {
    const response = await this._service.GetGeneralList();
    if (!response.success) return;
    this.people = response.data!;
  }
}
