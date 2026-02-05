import { Component, input } from '@angular/core';
import { UserDTO } from '../dtos/UserDTO';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";

@Component({
  selector: 'users-details',
  imports: [UiInputComponent],
  templateUrl: './users-details.component.html',
  styleUrl: './users-details.component.scss',
})
export class UsersDetailsComponent {
  user = input.required<UserDTO>();

  get roles() { return this.user().roles; }
  get isActive() { return this.user().isActive; }
}
