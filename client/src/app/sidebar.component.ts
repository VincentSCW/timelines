import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Timeline } from './models/timeline.model';
import { TimelineService } from './services/timeline.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html'
})

export class SidebarComponent implements OnInit {
  timelines$: Observable<Timeline[]>;

  constructor(private timelineService: TimelineService) {

  }

  ngOnInit() {
    this.timelines$ = this.timelineService.getTimelines();
  }
}