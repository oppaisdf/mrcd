import { Component, EventEmitter, input, Input, Output } from '@angular/core';
import { DefaultEntityStatusResponse } from '../../models/responses/person';
import { DocService } from '../../services/doc.service';

@Component({
  selector: 'people-comp-docs',
  standalone: false,
  templateUrl: './docs.component.html',
  styleUrl: './docs.component.sass'
})
export class DocsComponent {
  constructor(
    private _service: DocService
  ) { }

  @Input() documents: DefaultEntityStatusResponse[] = [];
  @Input() id = 0;
  @Input() isLoading = false;
  @Output() isLoadingChange = new EventEmitter<boolean>();

  message = '';
  success = true;

  async AssignAsync(
    doc: DefaultEntityStatusResponse
  ) {
    if (this.isLoading) return;
    this.isLoading = true;
    this.isLoadingChange.emit(true);

    const response = !doc.isActive ?
      await this._service.AssignAsync(this.id, doc.id) :
      await this._service.UnassignAsync(this.id, doc.id);
    this.message = response.message;
    this.success = response.success;
    if (response.success) doc.isActive = !doc.isActive;
    else doc.isActive = doc.isActive;

    this.isLoading = false;
    this.isLoadingChange.emit(false);
  }
}
