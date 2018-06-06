import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';
import { Moment } from '../models/moment.model';
import { Timeline } from '../models/timeline.model';

@Injectable()
export class TimelineService {
    private baseUrl: string;
    constructor(private http: HttpClient) {
        this.baseUrl = environment.apiServerUrl;
    }

    getMoments(topic: string): Observable<Moment[]> {
        return this.http.get<Moment[]>(`${this.baseUrl}/api/Moments?timeline=${topic}`);
    }

    insertOrReplaceMoment(moment: Moment): Observable<Moment> {
        return this.http.post<Moment>(`${this.baseUrl}/api/Moments`, moment);
    }

    deleteMoment(topic: string, date: Date): Observable<{}> {
        return this.http.delete(`${this.baseUrl}/api/Moments/${topic}/${date}`)
    }

    getTimelines(): Observable<Timeline[]> {
        return this.http.get<Timeline[]>(`${this.baseUrl}/api/Timelines`);
    }

    insertOrReplaceTimeline(timeline: Timeline): Observable<Timeline> {
        return this.http.post<Timeline>(`${this.baseUrl}/api/Timelines`, timeline);
    }

    deleteTimeline(topic: string): Observable<{}> {
        return this.http.delete(`${this.baseUrl}/api/Timelines/${topic}`);
    }
}