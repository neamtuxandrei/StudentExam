import { ChangeDetectorRef, Component } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { TestMessage } from './models/test-message';

import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'TestUI.SignalR';
  message: Subject<string> = new Subject<string>();
  
  myStringMessage: string = '';
  testMessage: TestMessage={Id : 0, Message: ""};
  constructor(public testService: SignalrService, private cdRef:ChangeDetectorRef)
  {
    
   
  }
  ngOnInit()
  {
    
    this.testService.startConnection();    
    this.testService.registerForMessages().subscribe((data)=>{
        this.testMessage = data;
    });
  }

  clickButton()
  {
    this.testService.sendMessage({Id:1, Message: "My Message"})
    ?.then(()=>console.log("Message sent"));

  }

  
}
