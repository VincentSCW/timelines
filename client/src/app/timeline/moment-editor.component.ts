import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'app-moment-editor',
    templateUrl: './moment-editor.component.html'
})

export class MomentEditorComponent {
    constructor(private dialogRef: MatDialogRef<MomentEditorComponent>) {

    }
}
