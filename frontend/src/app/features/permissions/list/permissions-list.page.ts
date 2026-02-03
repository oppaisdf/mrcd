import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { BaseEntitiesListPage } from "../../../shared/baseEntities/list/base-entities-list.page";

@Component({
  selector: 'app-permissions-list.page',
  imports: [RouterLink, BaseEntitiesListPage],
  templateUrl: './permissions-list.page.html',
  styleUrl: './permissions-list.page.scss',
})
export class PermissionsListPage {

}
