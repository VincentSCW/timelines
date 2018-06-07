import { Component, Output, EventEmitter } from '@angular/core';
import { MomentEditorComponent } from './timeline/moment-editor.component';

@Component({
	selector: 'app-navbar',
	templateUrl: './navbar.component.html'
})

export class NavbarComponent {
	@Output() toggleSidenav = new EventEmitter<void>();

	constructor() {

	}

	toggleMenu() {
		this.toggleSidenav.emit();
	}
}
