import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

import { MainComponent } from './main.component';
import { TimelineComponent } from './timeline/timeline.component';
import { SidebarComponent } from './sidebar.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: ':timeline', component: TimelineComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      preloadingStrategy: PreloadAllModules
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }