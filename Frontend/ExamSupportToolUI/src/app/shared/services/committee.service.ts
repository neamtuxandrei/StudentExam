import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CommitteeMember } from '../models/committee-member';
import { Observable } from 'rxjs';
import { observeNotification } from 'rxjs/internal/Notification';

@Injectable({
  providedIn: 'root'
})
export class CommitteeService {

  url = environment.baseUrl + '/api';

  constructor(private http: HttpClient) { }

  getCommitteeMembers(id: string): Observable<CommitteeMember[]> {
    
    
    return this.http.get<CommitteeMember[]>(this.url + '/secretary/CommitteeMembers/' + id);
  }

  getHeadOfCommitteeBySessionId(id:string){
    return this.http.get<CommitteeMember>(this.url + '/secretary/CommitteeMembers/' + id + '/HeadOfCommittee');
  }

  addCommitteeMember(values: any) {
    return this.http.post(this.url + '/secretary/CommitteeMembers/', values);
  }

  updateCommittee(id: string, values: any ) {
   return this.http.put(this.url + '/secretary/CommitteeMembers/' + id, values);
  }

  removeCommitteeMemberFromSession(committeeId:string,sessionId:string): Observable<any>{
    return this.http.delete(this.url+'/secretary/CommitteeMembers/'+committeeId+'/'+sessionId);
  }
}
