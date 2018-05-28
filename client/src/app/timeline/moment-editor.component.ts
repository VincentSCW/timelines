import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { Moment } from '../models/moment.model';
import { TimelineService } from '../services/timeline.service';

@Component({
    selector: 'app-moment-editor',
    templateUrl: './moment-editor.component.html',
    styleUrls: ['./moment-editor.component.css']
})

export class MomentEditorComponent implements OnInit {
    @Input() data: Moment;

    model: Moment = { topicKey: 'EF', recordDate: new Date() };

    constructor(private dialogRef: MatDialogRef<MomentEditorComponent>,
        private service: TimelineService) {

    }

    ngOnInit() {
        if (this.data != null) {
            this.model = this.data;
        }
    }

    onSubmit(newData: Moment) {
        //alert(JSON.stringify(newData));
        this.service.insertOrReplaceMoment(newData).toPromise()
            .then((moment) => this.dialogRef.close());
    }

    onCancel() {
        this.dialogRef.close();
    }
}
