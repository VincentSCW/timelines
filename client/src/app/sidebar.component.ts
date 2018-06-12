import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Observable';

import { Timeline } from './models/timeline.model';
import { TimelineService } from './services/timeline.service';
import { MomentEditorComponent } from './timeline/moment-editor.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  timelines$: Observable<Timeline[]>;
  urlTopicKey: string;
  editable$: Observable<boolean>;

  constructor(private timelineService: TimelineService,
    private authSvc: AuthService,
    private router: Router,
    private location: Location,
    private dialog: MatDialog) {
    this.editable$ = authSvc.isLoggedIn;
  }

  ngOnInit() {
    this.timelines$ = this.timelineService.getTimelines();
    this.urlTopicKey = location.pathname;
  }

  onTimelineClicked(timeline: Timeline) {
    this.router.navigateByUrl(`/timeline/${timeline.topicKey}`);
    this.urlTopicKey = timeline.topicKey;
  }

  onCreateTimelineClicked() {
    this.router.navigateByUrl('/timeline/create');
  }

  onAddMomentClicked() {
    if (this.urlTopicKey != null) {
      this.dialog.open(MomentEditorComponent, { data: { topicKey: this.urlTopicKey } });
    }
  }
}