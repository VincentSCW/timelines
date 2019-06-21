import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { MomentEditorComponent } from './timeline/moment-editor.component';
import { Observable } from 'rxjs/Observable';
import { Timeline } from './models/timeline.model';
import { TimelineService } from './services/timeline.service';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { MatDialog } from '@angular/material';
import { Subscription } from 'rxjs/Subscription';

@Component({
	selector: 'app-navbar',
	templateUrl: './navbar.component.html'
})

export class NavbarComponent implements OnInit, OnDestroy {
	timelines$: Observable<Timeline[]>;
	editable$: Observable<boolean>;
	activeTopicKey: string;
	burgerActive: boolean;

	private timelineSub: Subscription;

	constructor(private timelineService: TimelineService,
		private router: Router,
		private authSvc: AuthService) {
		this.editable$ = authSvc.isLoggedIn;
	}

	ngOnInit() {
		this.timelines$ = this.timelineService.getTimelines();
		this.timelineSub = this.timelineService.activeTimeline$.subscribe(t => this.activeTopicKey = t.topicKey);
	}

	ngOnDestroy() {
		if (!!this.timelineSub) { this.timelineSub.unsubscribe(); }
	}

	onTimelineClicked(timeline: Timeline) {
		this.router.navigateByUrl(`/timeline/${timeline.topicKey}`);
		this.timelineService.activeTimeline = timeline;
	}

	onCreateTimelineClicked() {
		this.router.navigateByUrl('/timeline/create');
	}

	onManageClick() {
		this.router.navigateByUrl('manage/images');
	}
}
