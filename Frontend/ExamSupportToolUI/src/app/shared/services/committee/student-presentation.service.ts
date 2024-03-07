import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { StudentPresentationForCommittee } from '../../models/committee/student-presentation-for-committee';

@Injectable({
  providedIn: 'root'
})
export class StudentPresentationService {
  url = `${environment.baseUrl}/api/committee`;

  constructor(private http: HttpClient) { }

  getStudentPresentations(examinationSessionId: string) {
    return this.http.get<StudentPresentationForCommittee[]>(`${this.url}/examination-sessions/${examinationSessionId}/StudentPresentation`);
  }

  getStudentPresentation(examinationSessionId: string) {
    return this.http.get<StudentPresentationForCommittee>(`${this.url}/examination-sessions/${examinationSessionId}/StudentPresentation`);
  }

}
