import { Component, OnInit, OnDestroy } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { ImageService } from "../services/image.service";

@Component({
  selector: 'app-image-manager',
  templateUrl: './image-manager.component.html',
  styleUrls: ['./image-manager.component.css']
})
export class ImageManagerComponent implements OnInit, OnDestroy {
  column1: string[] = new Array();
  column2: string[] = new Array();
  column3: string[] = new Array();

  private imagesSub: Subscription;

  constructor(private imageService: ImageService) {

  }

  ngOnInit() {
    this.imagesSub = this.imageService.getImageUrls().subscribe(imgs => {
      for (let i = 0; i < imgs.length; i++) {
        if (i % 3 == 0) { this.column1.push(imgs[i]); }
        else if (i % 3 == 1) { this.column2.push(imgs[i]); }
        else { this.column3.push(imgs[i]); }
      }
    });
  }

  ngOnDestroy() {
    if (!!this.imagesSub) { this.imagesSub.unsubscribe(); }
  }
}