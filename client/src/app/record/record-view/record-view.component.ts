import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { switchMap, flatMap } from 'rxjs/operators';
import { Subscription } from 'rxjs/Subscription';
import { NgxImageGalleryComponent, GALLERY_IMAGE, GALLERY_CONF } from "ngx-image-gallery";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-record-view',
  templateUrl: './record-view.component.html',
  providers: [DatePipe]
})
export class RecordViewComponent implements OnInit, OnDestroy {
  @ViewChild(NgxImageGalleryComponent) ngxImageGallery: NgxImageGalleryComponent;
  year: number;
  editable: boolean;

  conf: GALLERY_CONF = {
    imageOffset: '0px',
    showCloseControl: false,
    showDeleteControl: false,
    inline: true,
    backdropColor: 'white'
  }

  images: GALLERY_IMAGE[] = new Array<GALLERY_IMAGE>();

  private records$: Observable<Record[]>;
  private recordsSub: Subscription;
  
  constructor(private recordService: RecordService,
    private activatedRoute: ActivatedRoute,
    private title: Title,
    private datePipe: DatePipe) { }

  ngOnInit() {
    this.records$ = this.activatedRoute.paramMap.pipe(
      switchMap((params: ParamMap) => {
        this.year = parseInt(params.get('year'));
        return this.recordService.getRecords(this.year);
      })
    );

    this.recordsSub = this.records$.subscribe((r) => {
      let temp = [];
      r.forEach(e => {
        temp.push({
          url: e.imageUrl,
          thumbnailUrl: e.thumbnailUrl,
          title: `${this.datePipe.transform(e.date, 'yyyy/MM/dd')} ${e.title} @ ${e.location}`
        })
      });
      this.images = temp;
      this.title.setTitle(`刻 ${this.year} | 时间轴`);
    });
  }

  ngOnDestroy() {
    if (!!this.recordsSub) { this.recordsSub.unsubscribe(); }
  }
}
