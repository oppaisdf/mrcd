import { Component, OnInit } from '@angular/core';
import { DocService } from '../../services/doc.service';
import { DefaultResponse } from '../../../../core/models/responses/Response';
import { LoginService } from '../../../../services/login.service';

@Component({
  selector: 'documents-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: DocService,
    private _roles: LoginService
  ) { }

  docs: DefaultResponse[] = [];
  isAdm = true;
  showUpdater = false;
  selectedDocument: DefaultResponse = {
    id: 0,
    name: ''
  };

  async ngOnInit() {
    this.isAdm = this._roles.HasUserPermission('adm');

    const response = await this._service.GetAsync();
    if (!response.success) return;
    this.docs = response.data!;
  }

  SelectDocument(
    document: DefaultResponse
  ) {
    this.showUpdater = true;
    this.selectedDocument = document;
  }
}
