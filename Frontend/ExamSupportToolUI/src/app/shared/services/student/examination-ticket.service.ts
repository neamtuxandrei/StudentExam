import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ExaminationTicket } from '../../models/examination-ticket';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExaminationTicketService {
  baseUrl = environment.baseUrl + '/api/student';

  constructor(private http: HttpClient) { }

  getExaminationTicket() {
    return this.http.get<ExaminationTicket>(this.baseUrl + "/ExaminationTicket/");
  }
}
