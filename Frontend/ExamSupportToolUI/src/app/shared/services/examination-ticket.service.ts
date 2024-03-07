import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { ExaminationTicket } from "../models/examination-ticket";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class ExaminationTicketService {
  url = environment.baseUrl + "/api";
  
  constructor(private http: HttpClient) { }

  getTickets(id: string) {
    return this.http.get<ExaminationTicket[]>(this.url + "/secretary/ExaminationTickets/" + id);
  }
  removeTicket(id: string){
    return this.http.delete(this.url + "/secretary/ExaminationTickets/" + id);
  }
  importTicketsFromSession(toSessionId:string, fromSessionId:any)
  {
    return this.http.post(this.url + "/secretary/ExaminationTickets/bulk/" + toSessionId, fromSessionId )
  }

  addTicketToSession(values: any) {
    return this.http.post(this.url+"/secretary/ExaminationTickets",values);
  };

  updateTicket(id:string,values:any){
    return this.http.put(this.url+'/secretary/ExaminationTickets/'+id,values);
  }

  


}
