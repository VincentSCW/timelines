import { NgModule, Type } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { TocComponent } from './toc.component';

@NgModule({
  imports: [CommonModule, MatIconModule],
  declarations: [TocComponent],
  exports: [TocComponent],
  entryComponents: [TocComponent],
})
export class TocModule {

}
