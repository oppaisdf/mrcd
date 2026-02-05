import { Component, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

let nextId = 0;

@Component({
  selector: 'ui-input',
  imports: [],
  templateUrl: './ui-input.component.html',
  styleUrl: './ui-input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => UiInputComponent),
      multi: true
    }
  ]
})
export class UiInputComponent implements ControlValueAccessor {
  label = input.required<string>();
  type = input.required<'text' | 'password' | 'number'>();
  maxLength = input<number | null>();
  hint = input<string | null>(null);

  value = signal<string>('');
  id = `ui-input-${++nextId}`;
  disabled = signal<boolean>(false);

  private onChange: (v: any) => void = () => { };
  onTouched: () => void = () => { };

  writeValue(obj: any) {
    this.value.set(obj ?? '');
  }
  registerOnChange(fn: (v: any) => void) {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void) {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean) {
    this.disabled.set(isDisabled);
  }

  onInput(event: Event) {
    const element = event.target as HTMLInputElement;
    const rawValue = element.value;
    const value = this.type() === 'number'
      ? (rawValue === '' ? null : Number(rawValue))
      : rawValue;
    this.value.set(rawValue);
    this.onChange(value);
  }
}
