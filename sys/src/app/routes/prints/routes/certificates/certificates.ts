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
      officiant: [''],
      opacity: [0.2]
    });
  }

  form: FormGroup;
  people: ListGeneralResponse[] = [];
  backgroundImage?: string | null;

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

  onBackgroundSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      this.backgroundImage = reader.result as string; // dataURL
    };
    reader.readAsDataURL(file);
  }

  GetStyle() {
    const vertical = this.GetValue('isVertical') == 'true';
    return {
      width: vertical ? '794px' : '1123px',
      height: vertical ? '1123px' : '794px'
    };
  }
}
