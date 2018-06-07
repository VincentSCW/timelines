import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MainComponent } from './main.component';
import { TimelineComponent } from './timeline/timeline.component';
import { SidebarComponent } from './sidebar.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: 'timeline/:timeline', component: TimelineComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }