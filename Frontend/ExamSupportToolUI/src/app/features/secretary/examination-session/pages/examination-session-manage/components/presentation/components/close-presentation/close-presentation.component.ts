import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExaminationSession } from 'src/app/shared/models/examination-session';
import { SessionStatus } from 'src/app/shared/models/examination-session-status';
import { PresentationSchedule } from 'src/app/shared/models/presentation-schedule';
import { ExaminationSessionService } from 'src/app/shared/services/examination-session.service';

@Component({
  selector: 'app-close-presentation',
  templateUrl: './close-presentation.component.html',
  styleUrls: ['./close-presentation.component.css']
})
export class ClosePresentationComponent {
  
  examinationSession?: ExaminationSession;
  presentationSchedule?: PresentationSchedule;
  examinationSessionId: string = '';
  

  constructor(private router: Router, 
              private activatedRoute: ActivatedRoute,
              private examinationSessionService: ExaminationSessionService){
  }

  ngOnInit(){
    let id = this.activatedRoute.snapshot.parent?.parent?.paramMap.get('id');

    if (id) {
      this.examinationSessionId = id;
    }
  }
  onCancelClosingSessionClick() {
    this.router.navigate(['../start'], { relativeTo: this.activatedRoute });
  }
  onCloseSessionClick() {
    this.examinationSessionService.setExaminationSessionStatus(this.examinationSessionId, SessionStatus.Closed).subscribe();
    this.router.navigate(['../start'], { relativeTo: this.activatedRoute });
    //this.router.navigate(['../../../'], { relativeTo: this.activatedRoute });
  }

}
