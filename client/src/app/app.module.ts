import { NgModule } from '@angular/core';

import { SharedModule } from './shared.module';

import { AppRoutingModule } from './app-routing.module';
import { TimelineModule } from './timeline/timeline.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar.component';
import { SidebarComponent } from './sidebar.component';
import { MainComponent } from './main.component';
import { AccessKeyDialogComponent } from './timeline/access-key-dialog.component';
import { AuthGuard } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { TimelineService } from './services/timeline.service';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    NavbarComponent,
    SidebarComponent,
    AccessKeyDialogComponent
  ],
  imports: [
    SharedModule,
    TimelineModule,
    AppRoutingModule
  ],
  providers: [
    AuthService,
    AuthGuard,
    TimelineService
  ],
  entryComponents: [
    AccessKeyDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
