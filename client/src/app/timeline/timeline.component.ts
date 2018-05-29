import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { MatDialog } from '@angular/material';

import { TimelineService } from '../services/timeline.service';
import { Moment, GroupedMoments } from '../models/moment.model';
import { MomentEditorComponent } from './moment-editor.component';

import { environment } from '../../environments/environment';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit, OnDestroy {
  groupedMoments: GroupedMoments[];
  moments$: Observable<Moment[]>;
  editable = environment.editable;
  private momentsSubscription: Subscription;

  constructor(private timelineService: TimelineService, private dialog: MatDialog) { 
    this.groupedMoments = new Array();
  }

  ngOnInit() {
    this.moments$ = this.timelineService.getMoments('EF');
    this.momentsSubscription = this.moments$.subscribe(x => {
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
    });
  }

  ngOnDestroy() {
    if (!!this.momentsSubscription) { this.momentsSubscription.unsubscribe(); }
  }

  onEdit(moment: Moment) {
    this.dialog.open(MomentEditorComponent, {data: moment});
  }

  onDelete(moment: Moment) {
    this.timelineService.deleteMoment(moment.topicKey, moment.recordDate).toPromise();
  }
}
