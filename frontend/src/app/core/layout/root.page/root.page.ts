import { Component, inject } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { PopstateDirection } from '../../ui/transitions/popstate-direction';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './root.page.html',
  styleUrl: './root.page.scss',
})
export class RootPage {
  private readonly _pop = inject(PopstateDirection); // Registro para listener
}
