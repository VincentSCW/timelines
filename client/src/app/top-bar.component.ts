import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { MomentEditorComponent } from './timeline/moment-editor.component';

import { environment } from '../environments/environment';

@Component({
    selector: 'app-top-bar',
    templateUrl: './top-bar.component.html'
})

export class TopBarComponent {
    isMenuActive = false;
    editable = environment.editable;

    constructor(private dialog: MatDialog) {

    }

    toggleMenu() {
        this.isMenuActive = !this.isMenuActive;
    }

    onAddClicked() {
        this.dialog.open(MomentEditorComponent, {});
    }
}
