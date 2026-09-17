import { afterEveryRender, Component, ElementRef, inject, signal, viewChildren } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { SimpleParentResposne } from '../../parents/responses/simple-parent.response';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { UiPrintComponent } from '../../../core/ui/print/ui-print.component';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../../people/services/person.service';
import { GeneralListResponse } from '../../people/responses/general-list.response';

@Component({
  selector: 'app-print-certificate.page',
  imports: [ReactiveFormsModule, UiInputComponent, UiPrintComponent],
  templateUrl: './print-certificate.page.html',
  styleUrl: './print-certificate.page.scss',
})
export class PrintCertificatePage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(PersonService);
  private readonly today = new Date();
  readonly form = this._form.group({
    date: [`${this.today.getFullYear()}-${String(this.today.getMonth() + 1).padStart(2, '0')}-${String(this.today.getDate()).padStart(2, '0')}`, Validators.required],
    church: ['', Validators.required],
    signature: ['', Validators.required],
    cite: ['', Validators.required],
    officiant: ['', Validators.required],
    opacity: [0.2]
  });
  readonly backgroundImage = signal<string | null>(null);
  readonly imageError = signal<string | null>(null);
  private readonly contents = viewChildren<ElementRef<HTMLElement>>('certificateContent');
  private imageSelection = 0;

  constructor() {
    afterEveryRender(() => {
      for (const ref of this.contents()) {
        const content = ref.nativeElement;
        const available = content.parentElement!.clientHeight;
        const scale = Math.min(1, available / content.scrollHeight);
        content.style.transform = `translateX(-50%) scale(${scale})`;
      }
    });
  }

  async selectImage(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    const selection = ++this.imageSelection;
    this.imageError.set(null);
    if (!file) return;
    if (!file.type.startsWith('image/')) {
      this.imageError.set('Selecciona un archivo de imagen válido.');
      input.value = '';
      return;
    }
    try {
      const source = await new Promise<string>((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result as string);
        reader.onerror = () => reject(reader.error);
        reader.readAsDataURL(file);
      });
      const image = new Image();
      image.src = source;
      await image.decode();
      if (selection === this.imageSelection) this.backgroundImage.set(source);
    } catch {
      if (selection === this.imageSelection) {
        this.imageError.set('No se pudo leer la imagen. Prueba con otro archivo.');
        input.value = '';
      }
    }
  }

  removeImage(input: HTMLInputElement) {
    ++this.imageSelection;
    this.backgroundImage.set(null);
    this.imageError.set(null);
    input.value = '';
  }
  readonly people = signal<Array<GeneralListResponse>>([{
    personId: '',
    personName: 'Pato Exemploso Fabuloso Asombroso',
    isMasculine: false,
    isSunday: false,
    dob: new Date(),
    registrationDate: new Date(),
    parents: [],
    godparents: []
  }]);

  GetParents(
    parents: SimpleParentResposne[] | undefined
  ) {
    if (!parents) return '';
    return parents.reduce((a, b) => {
      return a += `${a.length > 0 ? ' y ' : ''}${b.name}`;
    }, '');
  }

  GetDOB(
    date: Date | string | null
  ) {
    if (!date) return '';
    const dob = typeof date === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(date)
      ? new Date(`${date}T00:00:00`) : new Date(date);
    if (Number.isNaN(dob.getTime())) return '';
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return 'Control not found :c';
    if (!control.touched || control.valid) return null;
    switch(controlName){
      case 'date': return 'La fecha es requerida';
      case 'church': return 'La parroquia es requerida';
      case 'signature': return 'La firma es requerida';
      case 'cite': return 'La cita es requerida';
      case 'officiant': return 'El celebrante es requerido';
      default: return null;
    }
  }

  async getAsync(){
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.generalListAsync();
    this._alert.clear();
    if (!response.isSuccess){
      this._alert.error(response.message);
      return;
    }
    this.people.set(response.data ?? []);
  }

  getValue(
    controlName: keyof typeof this.form.controls
  ){
    const control = this.form.get(controlName);
    if (!control) return null;
    return control.value;
  }
}
