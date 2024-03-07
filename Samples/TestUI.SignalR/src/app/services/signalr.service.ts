import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { TestMessage } from '../models/test-message';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {

private hubConnection?: signalR.HubConnection;
private lastMessage:BehaviorSubject<TestMessage> = new BehaviorSubject<TestMessage>({Id: 0, Message:''});
constructor() { }

  public startConnection()
  {
    this.hubConnection = new signalR.HubConnectionBuilder()
                                    .withUrl("https://localhost:7217/api/Test")
                                    .build();

    this.hubConnection.start()
    .then(() => {console.log("SignalR connection established.");})
    .catch(error => {console.log("Error while connecting to signalr hub: " + error);})   
                                 
    this.hubConnection.onclose((error)=>{console.log("Connection has been closed: " + error);})

  }

  
  public registerForMessages(): Observable<TestMessage>
  {
    this.hubConnection?.on('postnewmessage', (data)=>{
      let testMessage: TestMessage = JSON.parse(data)
      this.lastMessage.next(testMessage);      
    });

    return this.lastMessage.asObservable();
  }

  public sendMessage(message: TestMessage)
  {
    return this.hubConnection?.invoke("SendMessageToAll",message);
  }

}
