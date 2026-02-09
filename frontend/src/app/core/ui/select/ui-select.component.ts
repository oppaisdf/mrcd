import { Component, effect, ElementRef, forwardRef, input, signal, viewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SelectItem } from './SelectItem';

let nextId = 0;

@Component({
  selector: 'ui-select',
  imports: [],
  templateUrl: './ui-select.component.html',
  styleUrl: './ui-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => UiSelectComponent),
      multi: true
    }
  ]
})
export class UiSelectComponent implements ControlValueAccessor {
  items = input.required<Array<SelectItem<number>> | Array<SelectItem<string>> | Array<SelectItem<boolean>>>();
  label = input.required<string>();
  type = input.required<'string' | 'number' | 'boolean'>();

  value = signal<string>('');
  id = `ui-select-${++nextId}`;
  disabled = signal<boolean>(false);
  private readonly _select = viewChild<ElementRef<HTMLSelectElement>>('select');

  constructor() {
    effect(() => {
      const value = this.value();
      this.items();

      queueMicrotask(() => {
        const select = this._select()?.nativeElement;
        if (!select) return;
        select.value = value ?? '';
      });
    });
  }

  private onChange: (v: any) => void = () => { };
  onTouched: () => void = () => { };

  writeValue(obj: any): void {
    this.value.set(obj == null ? '' : String(obj));
  }
  registerOnChange(fn: (v: any) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }

  onSelectChange(
    event: Event
  ) {
    this.onTouched();
    const element = event.target as HTMLSelectElement;
    const rawValue = element.value;
    this.value.set(rawValue);

    if (rawValue === '') {
      this.onChange(null);
      return;
    }
    const type = this.type();
    const value = type === 'number'
      ? Number(rawValue)
      : type === 'boolean'
        ? rawValue.trim() === 'true'
        : rawValue;
    this.onChange(value);
  }
}
