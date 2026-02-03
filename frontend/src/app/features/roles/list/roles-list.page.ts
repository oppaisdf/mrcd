import { Component } from '@angular/core';
import { BaseEntitiesListPage } from "../../../shared/baseEntities/list/base-entities-list.page";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-roles-list.page',
  imports: [BaseEntitiesListPage, RouterLink],
  templateUrl: './roles-list.page.html',
  styleUrl: './roles-list.page.scss',
})
export class RolesListPage {
}
