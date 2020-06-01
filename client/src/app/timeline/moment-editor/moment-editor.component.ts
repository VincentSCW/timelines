import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, Inject, Injector } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Moment } from '../../models/moment.model';
import { TimelineService } from '../../services/timeline.service';

import { environment } from '../../../environments/environment';
import { Subscription } from 'rxjs';
import { Timeline } from '../../models/timeline.model';
import { DatePipe } from '@angular/common';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-moment-editor',
  templateUrl: './moment-editor.component.html'
})

export class MomentEditorComponent implements OnInit, OnDestroy {
  timeline: Timeline;
  public editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    minHeight: '200px',
    width: 'auto',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    sanitize: true,
    placeholder: 'Enter text here...',
    uploadUrl: `${environment.apiServerUrl}/api/images/upload`,
    toolbarHiddenButtons: [
      ['subscript',
        'superscript',
        'justifyLeft',
        'justifyCenter',
        'justifyRight',
        'justifyFull',
        'heading',
        'fontName'],
      ['fontSize',
        'textColor',
        'backgroundColor',
        'customClasses',
        'insertHorizontalRule',
        'toggleEditorMode']
    ]
  };

  private timelineSub: Subscription;

  constructor(
    private dialogRef: MatDialogRef<MomentEditorComponent>,
    private service: TimelineService,
    @Inject(MAT_DIALOG_DATA) public model: Moment) {
  }

  ngOnInit() {
    this.timelineSub = this.service.activeTimeline$.subscribe(t => {
      this.timeline = t;
      this.editorConfig.uploadUrl = this.editorConfig.uploadUrl + `?folder=${t.topicKey}`;
    });
  }

  ngOnDestroy() {
    if (!!this.timelineSub) { this.timelineSub.unsubscribe(); }
  }

  onSubmit(newData: Moment) {
    this.service.insertOrReplaceMoment(newData).toPromise()
      .then(() => {
        this.dialogRef.close(true);
      });
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}
