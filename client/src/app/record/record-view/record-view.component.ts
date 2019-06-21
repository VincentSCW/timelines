import { Component, OnInit, OnDestroy } from '@angular/core';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { switchMap } from 'rxjs/operators';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-record-view',
  templateUrl: './record-view.component.html',
  styleUrls: ['./record-view.component.scss']
})
export class RecordViewComponent implements OnInit, OnDestroy {
  records: Record[];
  year: number;

  private records$: Observable<Record[]>;
  private recordsSub: Subscription;
  
  constructor(private recordService: RecordService,
    private activatedRoute: ActivatedRoute,
    private title: Title) { }

  ngOnInit() {
    this.records$ = this.activatedRoute.paramMap.pipe(
      switchMap((params: ParamMap) => {
        this.year = parseInt(params.get('year'));
        return this.recordService.getRecords(this.year);
      })
    );

    this.recordsSub = this.records$.subscribe((r) => {
      this.records = r;
      this.title.setTitle('刻 | 时间轴');
    });
  }

  ngOnDestroy() {
    if (!!this.recordsSub) { this.recordsSub.unsubscribe(); }
  }
}
