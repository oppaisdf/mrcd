import { Component, inject } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { NavDirectionService } from '../../ui/transitions/nav-direction.service';
import { AlertComponent } from "../../../shared/alerts/alert.component/alert.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AlertComponent],
  templateUrl: './root.page.html',
  styleUrl: './root.page.scss',
})
export class RootPage {
  private readonly _navDir = inject(NavDirectionService); // Registro para listener
  private readonly _alert = inject(AlertService);
  readonly alertIsOpen = this._alert.isOpen;
}
