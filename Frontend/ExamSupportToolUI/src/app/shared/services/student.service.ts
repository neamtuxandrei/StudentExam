import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Student } from '../models/student';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  url = environment.baseUrl + "/api";

  constructor(private http: HttpClient) { }

  getStudents(id: string) {
    return this.http.get<Student[]>(this.url + "/secretary/Students/" + id);
  }

  addStudent(values: any) {
    return this.http.post(this.url + "/secretary/Students/", values);
  }

  updateStudent(id: string, values: any) {
    return this.http.put(this.url + "/secretary/Students/" + id, values)
  }

  removeStudent(studentId: string, examinationSessionId: string) {
    return this.http.delete(this.url + "/secretary/Students/", { params: { studentId, examinationSessionId } });
  }

  addStudentsBulk(id: string, values: any) {
    return this.http.post(this.url + "/secretary/Students/bulk/" + id, values);
  }
}
