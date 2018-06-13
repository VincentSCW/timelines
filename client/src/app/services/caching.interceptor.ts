import { Injectable } from "@angular/core";
import {
  HttpInterceptor, HttpRequest,
  HttpHandler, HttpHeaderResponse, HttpProgressEvent,
  HttpResponse, HttpSentEvent, HttpUserEvent
} from '@angular/common/http';
import { Observable } from "rxjs/Observable";
//import { of } from 'rxjs/operators';

@Injectable()
export class CachingInterceptor implements HttpInterceptor {
  constructor(private cache: RequestCache) { }

  intercept(req: HttpRequest<any>, next: HttpHandler)
    : Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
    return next.handle(req);
  }
}