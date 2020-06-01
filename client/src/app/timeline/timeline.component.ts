import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { Observable ,  Subscription } from 'rxjs';

import { TimelineService } from '../services/timeline.service';
import { Moment, GroupedMoments } from '../models/moment.model';
import { MomentEditorComponent } from './moment-editor/moment-editor.component';
import { Timeline, PeriodGroupLevel } from '../models/timeline.model';
import { AuthService } from '../services/auth.service';
import { DatePipe } from '@angular/common';
import { TimelineEditorComponent } from './timeline-editor/timeline-editor.component';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  providers: [DatePipe]
})
export class TimelineComponent implements OnInit, OnDestroy {
  timeline: Timeline;
  groupedMoments: GroupedMoments[];
  loaded: boolean;
  editable: boolean;
  align = -1;

  private timelineSubscription: Subscription;
  private momentsSubscription: Subscription;
  private editableSub: Subscription;
  private routeSub: Subscription;

  private timeline$: Observable<Timeline>;

  constructor(private timelineService: TimelineService,
    private authSvc: AuthService,
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private title: Title,
    private router: Router,
    private datePipe: DatePipe) { 
    this.groupedMoments = new Array();
    this.loaded = false;
  }

  ngOnInit() {
    this.routeSub = this.activatedRoute.paramMap.subscribe((params) => {
      this.timeline$ = this.timelineService.getTimeline(params.get('timeline'));
      this.refresh();
    });

    this.editableSub = this.authSvc.isLoggedIn$.subscribe(l => this.editable = l);
  }

  ngOnDestroy() {
    if (!!this.timelineSubscription) { this.timelineSubscription.unsubscribe(); }
    if (!!this.momentsSubscription) { this.momentsSubscription.unsubscribe(); }
    if (!!this.editableSub) { this.editableSub.unsubscribe(); }
    if (!!this.routeSub) { this.routeSub.unsubscribe(); }
  }

  refresh() {
    this.timelineSubscription = this.timeline$.subscribe((t) => {
      this.groupedMoments = new Array();
      this.loaded = false;

      this.timeline = t;
      this.timelineService.activeTimeline = this.timeline;
      this.title.setTitle(`${t.title} | 时间轴`);
      this.momentsSubscription = this.timelineService.getMoments(t.topicKey).subscribe(x => {
        x.map((m) => {
          this.groupByLevel(this.timeline.periodGroupLevel, m);
        });
        this.loaded = true;
      });
    });

  }

  onEdit(moment: Moment) {
    this.dialog.open(MomentEditorComponent, { data: moment })
      .afterClosed().toPromise().then(() => this.refresh());
  }

  onDelete(moment: Moment) {
    this.timelineService.deleteMoment(moment.topicKey, moment.recordDate).toPromise()
      .then(() => this.refresh());
  }

  groupByLevel(level: PeriodGroupLevel, m: Moment) {
    const date = new Date(m.recordDate);
    let groupKey: string;
    switch (level) {
      case PeriodGroupLevel.byDay:
        groupKey = this.datePipe.transform(date, 'MMMM dd, yyyy');
        break;
      case PeriodGroupLevel.byMonth:
        groupKey = this.datePipe.transform(date, 'MMMM yyyy');
        break;
      case PeriodGroupLevel.byYear:
        groupKey = this.datePipe.transform(date, 'yyyy');
        break;
    }

    const grouped = this.groupedMoments.find(g => g.group == groupKey);
    if (grouped == null) {
      this.groupedMoments.push({ group: groupKey, moments: [m] });
    } else {
      grouped.moments.push(m);
    }
  }

  onEditTimelineClicked() {
    this.dialog.open(TimelineEditorComponent, { data: this.timeline })
      .afterClosed().toPromise().then(() => this.refresh());
  }

  onDeleteTimelineClicked() {
    if (confirm('确定要删除吗？')) {
      this.timelineService.deleteTimeline(this.timeline.topicKey).toPromise();
    }
  }

  onAddMomentClicked() {
    this.dialog.open(MomentEditorComponent, { data: { topicKey: this.timeline.topicKey } })
      .afterClosed().toPromise().then(() => this.refresh());
  }
}
