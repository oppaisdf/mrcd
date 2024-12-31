import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashRoutingModule } from './dash-routing.module';
import { DashComponent } from './routes/core/dash.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';


@NgModule({
  declarations: [
    DashComponent,
    NavBarComponent
  ],
  imports: [
    CommonModule,
    DashRoutingModule
  ]
})
export class DashModule { }
