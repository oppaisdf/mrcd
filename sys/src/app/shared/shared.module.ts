import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertComponent } from './components/alert/alert.component';
import { UpdaterComponent } from './components/updater/updater.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateService } from './services/update.service';
import { PanExpandComponent } from './components/pan-expand/pan-expand.component';

@NgModule({
  declarations: [
    AlertComponent,
    UpdaterComponent,
    PanExpandComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  exports: [
    AlertComponent,
    UpdaterComponent,
    PanExpandComponent
  ],
  providers: [
    UpdateService
  ]
})
export class SharedModule { }
