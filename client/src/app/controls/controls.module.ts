import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { ButtonGroupComponent } from './button-group.component';

@NgModule({
  imports: [
    ReactiveFormsModule
  ],
  declarations: [
    ButtonGroupComponent
  ],
  exports: [
    ButtonGroupComponent
  ],
  entryComponents: [

  ]
})
export class ControlsModule {}
