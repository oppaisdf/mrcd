import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { BaseEntitiesListPage } from "../../../shared/baseEntities/list/base-entities-list.page";

@Component({
  selector: 'app-degrees-list.page',
  imports: [RouterLink, BaseEntitiesListPage],
  templateUrl: './degrees-list.page.html',
  styleUrl: './degrees-list.page.scss',
})
export class DegreesListPage {

}
