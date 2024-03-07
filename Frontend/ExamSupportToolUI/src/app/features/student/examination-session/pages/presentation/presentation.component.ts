import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, catchError, finalize, of, switchMap, tap } from 'rxjs';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { ExaminationSessionForStudent } from 'src/app/shared/models/student/examination-session-for-student';
import { ExaminationTicketForStudent } from 'src/app/shared/models/student/examination-ticket-for-student';
import { SignalRService } from 'src/app/shared/services/signal-r.service';
import { ExaminationSessionService } from 'src/app/shared/services/student/examination-session.service';
import { ExaminationTicketService } from 'src/app/shared/services/student/examination-ticket.service';

@Component({
  selector: 'app-presentation',
  templateUrl: './presentation.component.html',
  styleUrls: ['./presentation.component.css']
})
export class PresentationComponent {
  private examinationSessionSubject$ = new BehaviorSubject(true);
  private subscriptions = new SubscriptionsContainer();

  startDate = new Date();

  examinationSession$ = new Observable<ExaminationSessionForStudent>
  drawnTicket$: Observable<ExaminationTicketForStudent | null>;

  constructor(
    private studentExaminationSessionService: ExaminationSessionService,
    private studentExaminationTicketService: ExaminationTicketService,
    private signalRService: SignalRService,
    private messageService: MessageService
  ) {
    this.drawnTicket$ = of(null);
  }

  ngOnInit() {
    this.initObservables();
    this.loadSignalRSubscriptions();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  onGiveMeTicketButtonClick() {
    this.drawnTicket$ = this.studentExaminationTicketService.getExaminationTicket().pipe(
      finalize(() => {
        this.refreshExaminationSession();
      }),
      catchError((error) => {
        this.messageService.add({ severity: 'error', summary: 'Service Message', detail: 'There are no tickets available.' });
        return of();
      }),
    );
  }

  private loadSignalRSubscriptions() {
    this.subscriptions.add = this.signalRService.registerForNotification("onPresentationStart")
      .subscribe(() => {
        this.refreshExaminationSession();
      });

    this.subscriptions.add = this.signalRService.registerForNotification("onStudentStatusChange")
      .subscribe(() => {
        this.refreshExaminationSession();
      });

    this.subscriptions.add = this.signalRService.registerForNotification("onSessionStop")
      .subscribe(() => {
        this.refreshExaminationSession();
      });

      this.subscriptions.add = this.signalRService.registerForNotification("onScheduleUpdate")
      .subscribe(() => {
        this.refreshExaminationSession();
      });
  }

  private initObservables() {
    this.examinationSession$ = this.examinationSessionSubject$.asObservable().pipe(
      switchMap(() => this.studentExaminationSessionService.getExaminationSession()),
    )
  }

  private refreshExaminationSession() {
    this.examinationSessionSubject$.next(true);
  }


  calculateGeneralGrade(theoryGrade: number, projectGrade: number) {
    return (theoryGrade + projectGrade) / 2;
  }
}
