import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StudentPresentationService {
  baseUrl = environment.baseUrl + '/api/student/StudentPresentation';

  constructor(private http: HttpClient) { }

}
