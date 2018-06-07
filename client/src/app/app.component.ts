import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterEvent } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isOpened: boolean = false;

  constructor(private router: Router,
    private title: Title) {

  }

  ngOnInit() {
    this.router.events.filter((event) => event instanceof NavigationEnd)
      .subscribe((event: RouterEvent) => {
        if (this.isOpened == false) {
          this.isOpened = event.url == '' || event.url == '/';
        }
        this.title.setTitle('时间轴');
      });
  }
}
