import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterEvent } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isOpened: boolean = false;

  constructor(private router: Router) {

  }

  ngOnInit() {
    this.router.events.filter((event) => event instanceof NavigationEnd)
      .subscribe((event: RouterEvent) => {
        if (this.isOpened == false) {
          this.isOpened = event.url == '' || event.url == '/';
        }  
      });
  }
}
