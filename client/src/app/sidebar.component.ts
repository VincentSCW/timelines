import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Observable';

import { Timeline } from './models/timeline.model';
import { TimelineService } from './services/timeline.service';
import { MomentEditorComponent } from './timeline/moment-editor.component';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html'
})

export class SidebarComponent implements OnInit, OnDestroy {
  timelines$: Observable<Timeline[]>;
  activeTopicKey: string;
  editable$: Observable<boolean>;

  private timelineSub: Subscription;

  constructor(private timelineService: TimelineService,
    private authSvc: AuthService,
    private router: Router,
    private dialog: MatDialog) {
    this.editable$ = authSvc.isLoggedIn;
  }

  ngOnInit() {
    this.timelines$ = this.timelineService.getTimelines();
    this.timelineSub = this.timelineService.activeTimeline$.subscribe(t => this.activeTopicKey = t.topicKey);
  }

  ngOnDestroy() {
    if (!!this.timelineSub) { this.timelineSub.unsubscribe(); }
  }

  onTimelineClicked(timeline: Timeline) {
    this.router.navigateByUrl(`/timeline/${timeline.topicKey}`);
    this.timelineService.activeTimeline = timeline;
  }

  onCreateTimelineClicked() {
    this.router.navigateByUrl('/timeline/create');
  }

  onAddMomentClicked() {
    if (this.activeTopicKey != null) {
      this.dialog.open(MomentEditorComponent, { data: { topicKey: this.activeTopicKey } });
    }
  }

  onImageClicked() {
    this.router.navigateByUrl('manage/images');
    this.activeTopicKey = 'images';
  }
}