import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';
import { Moment } from '../models/moment.model';

@Injectable()
export class TimelineService {
    private baseUrl: string;
    constructor(private http: HttpClient) {
        this.baseUrl = environment.apiServerUrl;
    }

    getMoments(topic: string): Observable<Moment[]> {
        return this.http.get<Moment[]>(`${this.baseUrl}/api/Moments?timeline=${topic}`);
    }
}