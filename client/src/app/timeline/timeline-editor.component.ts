import { Component, OnInit } from '@angular/core';
import { Timeline, ProtectLevel, PeriodGroupLevel } from '../models/timeline.model';

@Component({
  selector: 'app-timeline-editor',
  templateUrl: './timeline-editor.component.html'
})
export class TimelineEditorComponent implements OnInit {
  model: Timeline = {
    topicKey: '',
    protectLevel: ProtectLevel.public,
    periodGroupLevel: PeriodGroupLevel.any,
    isCompleted: false
  };

  ngOnInit() {

  }
}
