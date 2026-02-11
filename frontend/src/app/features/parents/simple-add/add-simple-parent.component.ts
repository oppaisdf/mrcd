import { Component, input, model } from '@angular/core';
import { CreateSimpleParentRequest } from '../requests/create-simple-parent.request';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { CreateParentRequest } from '../requests/create-parent.request';
import { ParentFormComponent } from "../form/parent-form.component";

@Component({
  selector: 'parents-add-simple',
  imports: [ParentFormComponent],
  templateUrl: './add-simple-parent.component.html',
  styleUrl: './add-simple-parent.component.scss',
})
export class AddSimpleParentComponent {
  parents = model.required<Array<CreateSimpleParentRequest>>();
  title = input.required<string>();

  get fullParents() { return this.parents().length === 2; }
  readonly genders: Array<SelectItem<boolean>> = [{
    label: 'Masculino',
    value: true
  }, {
    label: 'Femenino',
    value: false
  }];

  removeParent(
    parent: CreateSimpleParentRequest
  ) {
    const parents = this.parents();
    const index = parents.indexOf(parent);
    parents.splice(index, 1);
    this.parents.set(parents);
  }

  addParent(
    request: CreateParentRequest
  ) {
    if (this.fullParents) return;

    const parents = this.parents();
    const parent: CreateSimpleParentRequest = {
      name: request.parentName,
      isMasculine: request.isMasculine,
      phone: request.phone
    };
    parents.push(parent);
    this.parents.set(parents);
  }
}
