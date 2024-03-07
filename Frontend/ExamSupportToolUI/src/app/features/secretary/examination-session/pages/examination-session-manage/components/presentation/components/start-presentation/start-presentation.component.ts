import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { ExaminationSession } from 'src/app/shared/models/examination-session';
import { PresentationSchedule } from 'src/app/shared/models/presentation-schedule';
import { PresentationScheduleStudent } from 'src/app/shared/models/presentation-schedule-student';
import { ExaminationSessionService } from 'src/app/shared/services/examination-session.service';
import { PresentationScheduleService } from 'src/app/shared/services/presentation-schedule.service';
import { StudentPresentation } from 'src/app/shared/models/student-presentation';
import { StudentPresentationService } from 'src/app/shared/services/student-presentation.service';
import { SignalRService } from 'src/app/shared/services/signal-r.service';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { Student } from 'src/app/shared/models/student';

@Component({
  selector: 'app-start-presentation',
  templateUrl: './start-presentation.component.html',
  styleUrls: ['./start-presentation.component.css']
})
export class StartPresentationComponent {
  subscriptions = new SubscriptionsContainer();

  presentationSchedule?: PresentationSchedule
   remainingStudents?: Student[]
  selectedNextStudent?: string;

  remainingStudentsSubject$ = new BehaviorSubject(true);
  remainingStudents$!: Observable<Student[]>

  presentationSchedule$!: Observable<PresentationSchedule>
  examinationSession$!: Observable<ExaminationSession>;
  examinationSessionSubject$ = new BehaviorSubject(true);

  startDate = new Date();
  examinationSessionId: any;

  isAbsent: boolean = false;
  examinationSessionHasSchedule: boolean = false;

  constructor(
    private messageService: MessageService,
    private examinationSessionService: ExaminationSessionService,
    private presentationScheduleService: PresentationScheduleService,
    private studentPresentationService: StudentPresentationService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private signalRService: SignalRService
  ) { }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  ngOnInit(): void {
    this.loadSessionId();
    this.loadExaminationSession();
    this.loadHasSchedule();

    this.remainingStudents$ = this.remainingStudentsSubject$.asObservable().pipe(
      switchMap(() => this.presentationScheduleService.getRemainingStudentToPresent(this.examinationSessionId)),
      tap(val=> this.remainingStudents = val)
    )

    this.signalRService.registerForNotification("onTicketDrawn").subscribe(() => {
      this.refreshExaminationSession();
    });
  }

  onNextStudentClick() {
    this.isAbsent = false;
    if (this.selectedNextStudent) {
      this.subscriptions.add = this.studentPresentationService.setNextPresentingStudent(this.examinationSessionId, this.selectedNextStudent)
        .subscribe(() => {
          this.refreshExaminationSession();
          this.refreshRemainingStudents();
        });
      }

      if(this.remainingStudents?.length === 0){
        this.router.navigate(['../close'], { relativeTo: this.activatedRoute });
      }
  }

  private refreshRemainingStudents() {
    this.remainingStudentsSubject$.next(true);
  }

  private loadExaminationSession() {
    this.examinationSession$ = this.examinationSessionSubject$.asObservable().pipe(
      switchMap(() => this.examinationSessionService.getExaminationSessionForPresentation(this.examinationSessionId)),
    );
  }


  private loadHasSchedule() {
    this.subscriptions.add = this.presentationScheduleService.getPresentationScheduleState(this.examinationSessionId).subscribe((value)=>{
      this.examinationSessionHasSchedule = value;
    });
  }

  private loadSessionId() {
    let id = this.activatedRoute.snapshot.parent?.parent?.paramMap.get('id');
    if (id) {
      this.examinationSessionId = id;
    }
  }

  private refreshExaminationSession() {
    this.examinationSessionSubject$.next(true);
  }

  onClosePresentationClick() {
    this.router.navigate(['../close'], { relativeTo: this.activatedRoute });
  }

  onStartPresentationClick() {
    this.subscriptions.add = this.examinationSessionService.setExaminationSessionStatus(this.examinationSessionId, 2)
      .subscribe({
        complete: () => {
          this.refreshExaminationSession();
        },
      });
  }

  onManagePresentationClick() {
    if (!this.examinationSessionHasSchedule) { // has no presentation schedule
      // generate presentation schedule with default options
      this.subscriptions.add = this.presentationScheduleService.generatePresentationSchedule(this.examinationSessionId, {}).subscribe(
        () => {
          this.router.navigate(['../manage'], { relativeTo: this.activatedRoute });
        });
    } else {
      this.router.navigate(['../manage'], { relativeTo: this.activatedRoute });
    }
  }

  onAbsentCheckboxChange(studentPresentationId: string) {
    if (studentPresentationId != undefined) {
      let payload = {
        "id": studentPresentationId,
        "isAbsent": this.isAbsent
      }
      this.studentPresentationService.updateAbsentStatus(this.examinationSessionId, payload).subscribe();
    }
  }
}
