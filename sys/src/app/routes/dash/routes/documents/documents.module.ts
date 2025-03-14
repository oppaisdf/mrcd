import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DocumentsRoutingModule } from './documents-routing.module';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailsComponent } from './routes/details/details.component';
import { DocService } from './services/doc.service';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';

@NgModule({
  declarations: [
    AllComponent,
    NewComponent,
    DetailsComponent
  ],
  imports: [
    CommonModule,
    DocumentsRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    DocService
  ]
})
export class DocumentsModule { }
