import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxEditorModule } from 'ngx-editor';

import { MaterialModule } from './material.module';

import { AppComponent } from './app.component';
import { TimelineComponent } from './timeline/timeline.component';
import { MomentEditorComponent } from './timeline/moment-editor.component';
import { TopBarComponent } from './top-bar.component';

@NgModule({
  declarations: [
    AppComponent,
    TimelineComponent,
    MomentEditorComponent,
    TopBarComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule,
    NgxEditorModule
  ],
  exports: [
    MaterialModule
  ],
  providers: [],
  entryComponents: [
    MomentEditorComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
