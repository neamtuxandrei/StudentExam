import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ExaminationSessionForStudent } from '../../models/student/examination-session-for-student';

@Injectable({
  providedIn: 'root'
})
export class ExaminationSessionService {

  baseUrl = environment.baseUrl + '/api/student';

  constructor(private http: HttpClient) { }

  getExaminationSession() {
    return this.http.get<ExaminationSessionForStudent>(this.baseUrl + "/ExaminationSession/");
  }
}
