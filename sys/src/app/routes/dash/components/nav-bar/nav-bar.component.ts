import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../../../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'dash-nav-bar',
  standalone: false,

  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.sass'
})
export class NavBarComponent implements OnInit {
  constructor(
    private _service: LoginService,
    private _router: Router
  ) { }

  burgerActive = false;
  isLogouting = false;
  isAdm = false;
  isSys = false;

  ngOnInit() {
    this.isAdm = this._service.HasUserPermission('adm');
    this.isSys = this._service.HasUserPermission('sys');
  }

  ShowMenu() {
    this.burgerActive = !this.burgerActive;
  }

  async Logout() {
    if (this.isLogouting) return;
    this.isLogouting = true;
    const response = await this._service.Logout();
    if (response.success) this._router.navigateByUrl('/login');
  }
}
