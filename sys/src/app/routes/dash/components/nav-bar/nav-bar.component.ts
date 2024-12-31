import { Component } from '@angular/core';

@Component({
  selector: 'dash-nav-bar',
  standalone: false,

  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.sass'
})
export class NavBarComponent {
  burgerActive = false;

  ShowMenu() {
    this.burgerActive = !this.burgerActive;
  }
}
