import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { TimelineService } from '../services/timeline.service';
import { Moment, GroupedMoments } from '../models/moment.model';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css'],
  providers: [TimelineService]
})
export class TimelineComponent implements OnInit {
  groupedMoments: GroupedMoments[];
  moments$: Observable<Moment[]>;
  private momentsSubscription: Subscription;

  constructor(private timelineService: TimelineService) { 
    this.groupedMoments = new Array();
  }

  ngOnInit() {
    this.moments$ = this.timelineService.getMoments('test_moment');
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

}
