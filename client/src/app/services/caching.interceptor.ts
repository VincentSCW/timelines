import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, } from "@angular/common/http";
//import { of } from 'rxjs/operators';

@Injectable()
export class CachingInterceptor implements HttpInterceptor {
  constructor(private cache: RequestCache) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return next.handle(req);
  }
}