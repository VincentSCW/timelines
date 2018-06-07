import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Observable';

import { environment } from '../environments/environment';
import { Timeline } from './models/timeline.model';
import { TimelineService } from './services/timeline.service';
import { MomentEditorComponent } from './timeline/moment-editor.component';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  timelines$: Observable<Timeline[]>;
  urlTopicKey: string;
  editable = environment.editable;

  constructor(private timelineService: TimelineService,
    private router: Router,
    private location: Location,
    private dialog: MatDialog) {

  }

  ngOnInit() {
    this.timelines$ = this.timelineService.getTimelines();
    this.urlTopicKey = location.pathname;
  }

  onTimelineClicked(timeline: Timeline) {
    this.router.navigateByUrl(`/${timeline.topicKey}`);
    this.urlTopicKey = timeline.topicKey;
  }

  onCreateTimelineClicked() {
    
  }

  onAddMomentClicked() {
    this.dialog.open(MomentEditorComponent, {});
  }
}