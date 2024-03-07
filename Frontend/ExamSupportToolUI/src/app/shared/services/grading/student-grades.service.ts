import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { StudentGrade } from '../../models/student-grade';

@Injectable({
  providedIn: 'root'
})
export class StudentGradesService {

  url = environment.baseUrl + '/api/committee';

  constructor(private http: HttpClient) { }

  getStudentsGrades(examinationSessionId?: string | null): Observable<StudentGrade[]>
  {
    return this.http.get<StudentGrade[]>(this.url + '/grades/' + examinationSessionId);
  }

  setStudentGrades(examinationSessionId: string, grade: StudentGrade): Observable<StudentGrade>
  {
      return this.http.post<StudentGrade>(this.url + '/grades/' + examinationSessionId, grade);
  }

}
