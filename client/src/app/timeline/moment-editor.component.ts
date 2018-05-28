import { Component, Output, EventEmitter } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { Moment } from '../models/moment.model';

@Component({
    selector: 'app-moment-editor',
    templateUrl: './moment-editor.component.html',
    styleUrls: ['./moment-editor.component.css']
})

export class MomentEditorComponent {
    @Output() submit = new EventEmitter<Moment>();
    @Output() cancel = new EventEmitter();

    model: Moment = { topic: '', recordDate: new Date()};

    constructor(private dialogRef: MatDialogRef<MomentEditorComponent>) {

    }

    onSubmit(data: Moment) {
        this.submit.emit(data);
    }

    onCancel() {
        this.cancel.emit();
    }
}
