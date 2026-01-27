import { Component, inject } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { NavDirectionService } from '../../ui/transitions/nav-direction.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './root.page.html',
  styleUrl: './root.page.scss',
})
export class RootPage {
  private readonly _navDir = inject(NavDirectionService); // Registro para listener
}
