import { NgModule } from '@angular/core';
import { Http } from '@angular/http';

import { SharedModule } from './shared.module';

import { AppRoutingModule } from './app-routing.module';
import { TimelineModule } from './timeline/timeline.module';
import { AccountModule } from './account/account.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar.component';
import { MainComponent } from './main.component';
import { AccessKeyDialogComponent } from './timeline/access-key-dialog/access-key-dialog.component';
import { AuthGuard } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { TimelineService } from './services/timeline.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { ImageService } from './services/image.service';
import { ManagementModule } from './management/management.module';
import { FooterComponent } from './footer.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    NavbarComponent,
    FooterComponent,
    AccessKeyDialogComponent
  ],
  imports: [
    SharedModule,
    TimelineModule,
    AccountModule,
    ManagementModule,
    AppRoutingModule
  ],
  providers: [
    AuthService,
    AuthGuard,
    TimelineService,
    ImageService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  entryComponents: [
    AccessKeyDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
