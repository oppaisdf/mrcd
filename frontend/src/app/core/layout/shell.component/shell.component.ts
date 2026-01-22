import { Component, DOCUMENT, inject } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { NavComponent } from "../nav.component/nav.component";

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, NavComponent],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
})
export class ShellComponent {
  private readonly _doc = inject(DOCUMENT);
  protected year = new Date().getFullYear();

  toggleTheme() {
    const root = this._doc.documentElement;
    const next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
    root.setAttribute('data-theme', next);
    sessionStorage.setItem('theme', next);
  }
}
