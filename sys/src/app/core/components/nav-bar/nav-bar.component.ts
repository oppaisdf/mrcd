import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../../../services/login.service';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.sass'
})
export class NavBarComponent {
  constructor(
    private _service: LoginService,
    private _router: Router
  ) { }

  burgerActive = false;
  isLogouting = false;

  ShowMenu() {
    this.burgerActive = !this.burgerActive;
  }

  async Logout() {
    if (this.isLogouting) return;
    this.isLogouting = true;
    const response = await this._service.Logout();
    this.isLogouting = false;
    if (response.success) this._router.navigateByUrl('/login');
  }
}
