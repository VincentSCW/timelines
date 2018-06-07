import { Component, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material';
import { MomentEditorComponent } from './timeline/moment-editor.component';

@Component({
	selector: 'app-navbar',
	templateUrl: './navbar.component.html'
})

export class NavbarComponent {
	@Output() toggleSidenav = new EventEmitter<void>();

	constructor(private dialog: MatDialog) {

	}

	toggleMenu() {
		this.toggleSidenav.emit();
	}

	onAddClicked() {
		this.dialog.open(MomentEditorComponent, {});
	}
}
