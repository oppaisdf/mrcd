import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges
} from '@angular/core';
import {
  finalize,
  interval,
  Subject,
  takeUntil,
  tap,
  timer
} from 'rxjs';

@Component({
  selector: 'shared-alert',
  standalone: false,
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.sass'
})
export class AlertComponent implements OnChanges, OnDestroy {
  @Input() message = '';
  @Output() readonly messageChange = new EventEmitter<string>();
  @Input() success = false;
  progress = 0;
  private readonly _stop$ = new Subject<void>();

  ngOnDestroy() {
    this._stop$.next();
    this._stop$.complete();
  }

  private Reset() {
    this.progress = 0;
    this._stop$.next();
  }

  ngOnChanges(
    changes: SimpleChanges
  ) {
    this.Reset();
    if (!changes['message'] || !this.message) return;

    interval(100)
      .pipe(
        takeUntil(timer(5000)),
        takeUntil(this._stop$),
        tap(() => (this.progress = Math.min(this.progress + 2, 100))),
        finalize(() => this.messageChange.emit(''))
      )
      .subscribe();
  }

  Hide() {
    this.Reset();
    this.message = '';
    this.messageChange.emit('');
  }

  GetClass() {
    return {
      'is-success': this.success,
      'is-danger': !this.success
    };
  }
}
