import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ExaminationSession } from '../models/examination-session';
import { SessionStatus } from '../models/examination-session-status';

@Injectable({
  providedIn: 'root',
})
export class ExaminationSessionService {
  baseUrl = environment.baseUrl + '/api';
  constructor(private http: HttpClient) { }


  getExaminationSessions() {
    return this.http.get<ExaminationSession[]>(this.baseUrl + "/secretary/ExamSessions");
  }

  getExaminationSession(id: string) {
    return this.http.get<ExaminationSession>(this.baseUrl + "/secretary/ExamSessions/" + id);
  }

  getExaminationSessionStatus(id: string) {
    return this.http.get<number>(this.baseUrl + "/secretary/ExamSessions/" + id + "/status");
  }

  setExaminationSessionStatus(id: string, status: SessionStatus) {
    return this.http.put<ExaminationSession>(this.baseUrl + "/secretary/ExamSessions/" + id, status);
  }

  addExaminationSession(values: any) {
    return this.http.post(this.baseUrl + "/secretary/ExamSessions/", values);
  }

  getExaminationSessionForPresentation(id: string) {
    return this.http.get<ExaminationSession>(this.baseUrl + "/secretary/ExamSessions/" + id + "/presentation")
  }
}
