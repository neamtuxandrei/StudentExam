import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PresentationSchedule } from '../../models/presentation-schedule';

@Injectable({
  providedIn: 'root'
})
export class PresentationScheduleService {
  url = `${environment.baseUrl}/api`;

  constructor(private http: HttpClient) { }

  getPresentationSchedule(examinationSessionId: string) {
    return this.http.get<PresentationSchedule>(`${this.url}/committee/examination-sessions/${examinationSessionId}/PresentationSchedule`);
  }

}
