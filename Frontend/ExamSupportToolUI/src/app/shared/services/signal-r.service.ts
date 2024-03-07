import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseUrl}/api/signalr`)
      .build();

    this.startHubConnections();
  }
  
  registerForNotification(eventName: string): Observable<any> {
    return new Observable<any>((observer) => {
      this.hubConnection.on(eventName, (data: any) => {
        observer.next(data);
      });
    });
  }

  private startHubConnections() {
    this.hubConnection.start().catch((err: any) => console.error(`SignalRHub: ${err}`));
  }
}
