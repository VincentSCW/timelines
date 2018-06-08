import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxEditorModule } from 'ngx-editor';

import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './material.module';
import { ControlsModule } from './controls/controls.module';

import { AppComponent } from './app.component';
import { TimelineComponent } from './timeline/timeline.component';
import { MomentEditorComponent } from './timeline/moment-editor.component';
import { TimelineEditorComponent } from './timeline/timeline-editor.component';
import { NavbarComponent } from './navbar.component';
import { SidebarComponent } from './sidebar.component';
import { MainComponent } from './main.component';
import { AccessKeyDialogComponent } from './timeline/access-key-dialog.component';
import { TimelineService } from './services/timeline.service';
import { TimelineAccessGuard } from './services/timeline-access-guard.service';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    TimelineComponent,
    MomentEditorComponent,
    TimelineEditorComponent,
    NavbarComponent,
    SidebarComponent,
    AccessKeyDialogComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule,
    NgxEditorModule,
    ControlsModule,
    AppRoutingModule
  ],
  exports: [
    MaterialModule
  ],
  providers: [
    TimelineService,
    TimelineAccessGuard
  ],
  entryComponents: [
    MomentEditorComponent,
    AccessKeyDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
