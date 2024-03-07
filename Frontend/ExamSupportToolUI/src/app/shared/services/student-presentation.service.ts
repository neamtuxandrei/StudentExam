import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { StudentPresentation } from '../models/student-presentation';

@Injectable({
  providedIn: 'root'
})
export class StudentPresentationService {

  url = `${environment.baseUrl}/api`;

  constructor(private http: HttpClient) { }

  getStudentPresentations(examinationSessionId: string) {
    return this.http.get<StudentPresentation[]>(`${this.url}/secretary/examination-sessions/${examinationSessionId}/StudentPresentation/list`);
  }

  updateAbsentStatus(examinationSessionId: string, values: any) {
    return this.http.put(`${this.url}/secretary/examination-sessions/${examinationSessionId}/StudentPresentation`, values);
  }

  setNextPresentingStudent(examinationSessionId: string, nextPresentingStudent: string) {
    return this.http.get(`${this.url}/secretary/examination-sessions/${examinationSessionId}/StudentPresentation/${nextPresentingStudent}/next-student`);
  }
}
