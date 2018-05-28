import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { MomentEditorComponent } from './timeline/moment-editor.component';

@Component({
    selector: 'app-top-bar',
    templateUrl: './top-bar.component.html'
})

export class TopBarComponent {
    isMenuActive = false;

    constructor(private dialog: MatDialog) {

    }

    toggleMenu() {
        this.isMenuActive = !this.isMenuActive;
    }

    onAddClicked() {
        this.dialog.open(MomentEditorComponent, {});
    }
}
