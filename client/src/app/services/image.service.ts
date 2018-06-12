import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from "../../environments/environment";

@Injectable()
export class ImageService {
  private baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.apiServerUrl;
  }

  getImageUrls(timeline?: string): Observable<string[]> {
    return this.http.get<string[]>(timeline == null ? `${this.baseUrl}/api/images` : `${this.baseUrl}/api/images?timeline=${timeline}`);
  }
}