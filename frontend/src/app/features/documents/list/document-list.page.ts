import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BaseEntitiesListPage } from "../../../shared/baseEntities/list/base-entities-list.page";

@Component({
  selector: 'app-document-list.page',
  imports: [RouterLink, BaseEntitiesListPage],
  templateUrl: './document-list.page.html',
  styleUrl: './document-list.page.scss',
})
export class DocumentListPage {

}
