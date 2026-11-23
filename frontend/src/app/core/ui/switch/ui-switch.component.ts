import { Component, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

let nextId = 0;

@Component({
  selector: 'ui-switch',
  imports: [],
  templateUrl: './ui-switch.component.html',
  styleUrl: './ui-switch.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => UiSwitchComponent),
      multi: true
    }
  ]
})
export class UiSwitchComponent implements ControlValueAccessor {
  readonly label = input<string>('Estado');
  readonly activeLabel = input<string>('Activo');
  readonly inactiveLabel = input<string>('Inactivo');

  readonly id = `ui-switch-${++nextId}`;
  readonly value = signal(false);
  readonly disabled = signal(false);

  private onChange: (value: boolean) => void = () => { };
  onTouched: () => void = () => { };

  writeValue(value: boolean | null | undefined): void {
    this.value.set(value === true);
  }

  registerOnChange(fn: (value: boolean) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }

  onToggle(event: Event): void {
    if (this.disabled()) return;
    const checked = (event.target as HTMLInputElement).checked;
    this.value.set(checked);
    this.onChange(checked);
  }
}
