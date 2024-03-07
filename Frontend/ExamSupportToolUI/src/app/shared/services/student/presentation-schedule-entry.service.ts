import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PresentationScheduleEntry } from '../../models/presentation-schedule';

@Injectable({
  providedIn: 'root'
})
export class PresentationScheduleEntryService {

  baseUrl = environment.baseUrl + '/api/student';

  constructor(private http: HttpClient) { }

  getSchedule() {
    return this.http.get<PresentationScheduleEntry>(this.baseUrl + "/PresentationSchedule/");
  }
}
