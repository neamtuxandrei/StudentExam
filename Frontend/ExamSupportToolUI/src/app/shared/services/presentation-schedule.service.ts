import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PresentationSchedule } from '../models/presentation-schedule';
import { StudentPresentation } from '../models/student-presentation';
import { Student } from '../models/student';

@Injectable({
  providedIn: 'root'
})
export class PresentationScheduleService {
  url = `${environment.baseUrl}/api`;

  constructor(private http: HttpClient) { }

  getPresentationSchedule(examinationSessionId: string) {
    return this.http.get<PresentationSchedule>(`${this.url}/secretary/examination-sessions/${examinationSessionId}/PresentationSchedule`);
  }

  generatePresentationSchedule(examinationSessionId: string, values: any) {
    return this.http.post(`${this.url}/secretary/examination-sessions/${examinationSessionId}/PresentationSchedule`, values);
  }

  getRemainingStudentToPresent(examinationSessionId: string) {
    return this.http.get<Student[]>(`${this.url}/secretary/examination-sessions/${examinationSessionId}/PresentationSchedule/remaining-students`);
  }

  movePresentationScheduleEntry(examinationSessionId: string, values: any) {
    return this.http.put(`${this.url}/secretary/examination-sessions/${examinationSessionId}/PresentationSchedule`, values);
  }

  getPresentationScheduleState(examinationSessionId: string){
    return this.http.get<boolean>(`${this.url}/secretary/examination-sessions/${examinationSessionId}/PresentationSchedule/status`);
  }
}
