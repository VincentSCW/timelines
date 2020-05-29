import { NgModule } from '@angular/core';
import { SharedModule } from '../shared.module';
import { TimelineRoutingModule } from './timeline-routing.module';

import { TimelineComponent } from '../timeline/timeline.component';
import { MomentEditorComponent } from './moment-editor/moment-editor.component';
import { ActionPanelComponent } from './action-panel/action-panel.component';
import { TimelineEditorComponent } from './timeline-editor/timeline-editor.component';
import { TimelineAccessGuard } from '../services/timeline-access-guard.service';

@NgModule({
  imports: [
    SharedModule,
    TimelineRoutingModule
  ],
  declarations: [
    TimelineComponent,
    MomentEditorComponent,
    ActionPanelComponent,
    TimelineEditorComponent
  ],
  providers: [
    TimelineAccessGuard
  ],
  entryComponents: [
    MomentEditorComponent
  ]
})
export class TimelineModule { }