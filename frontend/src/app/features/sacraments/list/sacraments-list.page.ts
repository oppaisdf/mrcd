import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { BaseEntitiesListPage } from "../../../shared/baseEntities/list/base-entities-list.page";

@Component({
  selector: 'app-sacraments-list.page',
  imports: [RouterLink, BaseEntitiesListPage],
  templateUrl: './sacraments-list.page.html',
  styleUrl: './sacraments-list.page.scss',
})
export class SacramentsListPage {

}
