import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ExaminationSession } from '../../models/examination-session';

@Injectable({
  providedIn: 'root'
})
export class ExaminationSessionService {

  baseUrl = environment.baseUrl + '/api/committee';

  constructor(private http: HttpClient) { }

  getExaminationSessions() {
    return this.http.get<ExaminationSession[]>(this.baseUrl + "/ExaminationSession/");
  }
}
