import { Component } from '@angular/core';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { ExaminationSession } from 'src/app/shared/models/examination-session';
import { ExaminationSessionService } from 'src/app/shared/services/committee/examination-session.service';
import { SessionStatus } from 'src/app/shared/models/examination-session-status';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { CommitteeService } from 'src/app/shared/services/committee.service';

@Component({
  selector: 'app-examination-session-list',
  templateUrl: './examination-session-list.component.html',
  styleUrls: ['./examination-session-list.component.css']
})
export class ExaminationSessionListComponent {
  subscriptions = new SubscriptionsContainer();
  examinationSessionListSubject = new BehaviorSubject(true);

  examinationSessionList$?: Observable<ExaminationSession[]>;
  examinationSessionList: ExaminationSession[] = [];

  sessionStatusType = SessionStatus;

  constructor(
    private messageService: MessageService,
    private examinationSessionService: ExaminationSessionService,
    private committeeService: CommitteeService,
    private router: Router,
    private route: ActivatedRoute
  ) {}


  ngOnInit() {
    this.loadExaminationSessions();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  private loadExaminationSessions() {
    this.examinationSessionList$ = this.examinationSessionListSubject.asObservable().pipe(
      switchMap(() => this.examinationSessionService.getExaminationSessions()),
      tap(result => this.examinationSessionList = result)
    );
  }

  private refreshExaminationSessionsList() {
  this.examinationSessionListSubject.next(true);
  }

  checkStatus(sessionStatus: any): boolean {
    return (
      sessionStatus === this.sessionStatusType.Opened ||
      sessionStatus === this.sessionStatusType.Presenting
    );
  }

  getStatusClass(status: SessionStatus): string {
    switch (status) {
      case SessionStatus.Created:
        return 'created-color';
      case SessionStatus.Opened:
        return 'opened-color';
      case SessionStatus.Presenting:
        return 'presenting-color';
      case SessionStatus.Closed:
        return 'closed-color';
      default:
        return '';
    }
  }

  onCloseSessionClick(event:Event)
  {
    // to implement
    event.stopPropagation();
  }

  onOpenSessionClick(event:Event)
  {
    // to implement
    event.stopPropagation();
  }

  onSessionClick(id: string) {
    this.router.navigate(['../' + id], { relativeTo: this.route });
  }

}
