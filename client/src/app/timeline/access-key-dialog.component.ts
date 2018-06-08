import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Timeline } from '../models/timeline.model';
import { TimelineService } from '../services/timeline.service';

@Component({
  templateUrl: './access-key-dialog.component.html'
})
export class AccessKeyDialogComponent implements OnInit {
  model: Timeline;

  constructor(@Inject(MAT_DIALOG_DATA) public data: Timeline,
    private dialogRef: MatDialogRef<AccessKeyDialogComponent>,
    private timelineService: TimelineService) {
    
  }

  ngOnInit() {
    this.model = this.data;
  }

  async onSubmit(data: Timeline) {
    const isValid = await this.timelineService.verifyAccessCode(data).toPromise();
    if (isValid) {
      this.dialogRef.close();
    }  
    else {
      
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}