import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertComponent } from './components/alert/alert.component';
import { UpdaterComponent } from './components/updater/updater.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateService } from './services/update.service';

@NgModule({
  declarations: [
    AlertComponent,
    UpdaterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  exports: [
    AlertComponent,
    UpdaterComponent
  ],
  providers: [
    UpdateService
  ]
})
export class SharedModule { }
