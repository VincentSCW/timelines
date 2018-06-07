import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { TimelineService } from '../services/timeline.service';
import { Moment, GroupedMoments } from '../models/moment.model';
import { MomentEditorComponent } from './moment-editor.component';
import { Timeline } from '../models/timeline.model';

import { environment } from '../../environments/environment';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit, OnDestroy {
  timeline: Timeline;
  groupedMoments: GroupedMoments[];
  loaded: boolean;
  editable = environment.editable;

  private timelineSubscription: Subscription;
  private momentsSubscription: Subscription;

  constructor(private timelineService: TimelineService,
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private title: Title) { 
    this.groupedMoments = new Array();
    this.loaded = false;
  }

  ngOnInit() {
    const topicKey = this.activatedRoute.snapshot.paramMap.get('timeline');
    this.timelineSubscription = this.timelineService.getTimeline(topicKey).subscribe(x => {
      this.timeline = x;
      this.title.setTitle(`${x.title} | 时间轴`)
    });
    
    this.momentsSubscription = this.timelineService.getMoments(topicKey).subscribe(x => {
      x.map((m) => {
        const date = new Date(m.recordDate);
        let month = date.toLocaleDateString('en-US', { year: 'numeric', month: 'long'});
        let grouped = this.groupedMoments.find(g => g.group == month);
        if (grouped == null) {
          this.groupedMoments.push({group: month, moments: [m]});
        } else {
          grouped.moments.push(m);
        }
      });
      this.loaded = true;
    });
  }

  ngOnDestroy() {
    if (!!this.timelineSubscription) { this.timelineSubscription.unsubscribe(); }
    if (!!this.momentsSubscription) { this.momentsSubscription.unsubscribe(); }
  }

  onEdit(moment: Moment) {
    this.dialog.open(MomentEditorComponent, {data: moment});
  }

  onDelete(moment: Moment) {
    this.timelineService.deleteMoment(moment.topicKey, moment.recordDate).toPromise();
  }
}
