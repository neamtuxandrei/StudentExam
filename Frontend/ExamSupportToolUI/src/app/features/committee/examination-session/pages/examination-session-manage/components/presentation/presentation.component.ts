import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { StudentForCommittee } from 'src/app/shared/models/committee/student-for-committee';
import { StudentPresentationForCommittee } from 'src/app/shared/models/committee/student-presentation-for-committee';
import { PresentationSchedule, PresentationScheduleEntry, PresentationScheduleStudent } from 'src/app/shared/models/presentation-schedule';
import { StudentGrade } from 'src/app/shared/models/student-grade';
import { PresentationScheduleService } from 'src/app/shared/services/committee/presentation-schedule.service';
import { StudentPresentationService } from 'src/app/shared/services/committee/student-presentation.service';
import { StudentGradesService } from 'src/app/shared/services/grading/student-grades.service';
import { SignalRService } from 'src/app/shared/services/signal-r.service';

@Component({
  selector: 'app-presentation',
  templateUrl: './presentation.component.html',
  styleUrls: ['./presentation.component.css']
})
export class PresentationComponent {

  presentationSchedule: PresentationSchedule = {} as PresentationSchedule;
  presentationScheduleEntries: PresentationScheduleEntry[] = [];
  presentationScheduleByDate: any[] = []

  currentPresentation?: StudentPresentationForCommittee;
  currentStudentPresenting: StudentForCommittee = {} as StudentForCommittee;
  studentGrades: StudentGrade = {} as StudentGrade;

  examinationSessionId: any;
  graduationYear = new Date().getFullYear();

  subscriptions = new SubscriptionsContainer();
  students: PresentationScheduleStudent[] = [];

  absentForm = new FormGroup({
    absentStatus: new FormControl()
  });

  gradesForm = new FormGroup({
    theoryGrade: new FormControl(0, [Validators.required, Validators.pattern('^(?:[0-9]|10)$')]),
    projectGrade: new FormControl(0, [Validators.required, Validators.pattern('^(?:[0-9]|10)$')]),
  })

  constructor(
    private messageService: MessageService,
    private studentPresentationService: StudentPresentationService,
    private schedulePresentationService: PresentationScheduleService,
    private gradesService: StudentGradesService,
    private signalRService: SignalRService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    let id = this.activatedRoute.snapshot.parent?.parent?.paramMap.get('id');

    if (id) {
      this.examinationSessionId = id;
      this.getStudentPresentation(id);
      this.getPresentationSchedule(id);
    }
    this.loadSignalRSubscriptions();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  private loadSignalRSubscriptions() {
    this.subscriptions.add = this.signalRService.registerForNotification("onStudentStatusChange").subscribe(() => {
      this.getStudentPresentation(this.examinationSessionId);
      this.gradesForm.reset();
    });

    this.subscriptions.add = this.signalRService.registerForNotification("onTicketDrawn").subscribe(() => {
      this.getStudentPresentation(this.examinationSessionId);
    });

    this.subscriptions.add = this.signalRService.registerForNotification("onPresentationStart").subscribe(() => {
      this.getStudentPresentation(this.examinationSessionId);
    });

    this.subscriptions.add = this.signalRService.registerForNotification("onSessionStop").subscribe(() => {
      this.getStudentPresentation(this.examinationSessionId);
    });
  }

  getStudentPresentation(id: string) {
    this.subscriptions.add = this.studentPresentationService.getStudentPresentation(id)
      .subscribe(
        (presentation => {
          this.currentPresentation = presentation;
          if (presentation.student)
            this.currentStudentPresenting = presentation.student;
        }
        ));
  }

  getPresentationSchedule(id: string) {
    this.subscriptions.add = this.schedulePresentationService.getPresentationSchedule(id)
      .subscribe({
        next: result => {
          this.presentationSchedule = result;
          this.presentationScheduleEntries = this.presentationSchedule.presentationScheduleEntries;
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Something went wrong' });
        }
      });
  }

  public findNextStudentNameToPresent(): string {
    const messageIfNoStudentNext = 'There are currently no more students scheduled';
    if (
      this.presentationSchedule &&
      this.presentationSchedule.presentationScheduleEntries &&
      this.currentStudentPresenting
    ) {

      const currentIndex = this.presentationSchedule.presentationScheduleEntries.findIndex(
        (entry) => entry.student?.id === this.currentStudentPresenting.id
      );

      let nextIndex = currentIndex + 1;

      if(this.presentationSchedule.presentationScheduleEntries.at(currentIndex+1)?.student === null || undefined)
        nextIndex = currentIndex + 2;

      if (nextIndex < this.presentationSchedule.presentationScheduleEntries.length) {
        const nextStudentName = this.presentationSchedule.presentationScheduleEntries[nextIndex].student?.name;
        if (!nextStudentName) {
          return messageIfNoStudentNext;
        } else {
          return nextStudentName;
        }
      } else {
        return messageIfNoStudentNext;
      }
    } else {
      return messageIfNoStudentNext;
    }
  }
  public onInputChange() {
    let theoryGrade = this.gradesForm.get('theoryGrade')?.value ?? 0;
    let projectGrade = this.gradesForm.get('projectGrade')?.value ?? 0;

    if (theoryGrade > 10) {
      this.gradesForm.get('theoryGrade')?.patchValue(10);
    }
    if (theoryGrade < 0) {
      this.gradesForm.get('theoryGrade')?.patchValue(0);
    }
    if (projectGrade > 10) {
      this.gradesForm.get('projectGrade')?.patchValue(10);
    }
    if (projectGrade < 0) {
      this.gradesForm.get('projectGrade')?.patchValue(0);
    }
    if (theoryGrade - Math.floor(theoryGrade) != 0) {
      this.gradesForm.get('theoryGrade')?.patchValue(Math.floor(theoryGrade));
      this.gradesForm.get('projectGrade')?.patchValue(Math.floor(projectGrade));
    }

    if (this.gradesForm.valid && this.currentPresentation){
      this.studentGrades.id = this.currentPresentation?.id;
      this.studentGrades.name = this.currentStudentPresenting.name;
      this.studentGrades.committeeId = this.currentPresentation?.currentCommitteeId;
      this.studentGrades.diplomaProjectName = this.currentStudentPresenting.diplomaProjectName;
      this.studentGrades.coordinatorName = this.currentStudentPresenting.coordinatorName;
      this.studentGrades.theoryGrade = this.gradesForm.get('theoryGrade')?.value ?? 0;
      this.studentGrades.projectGrade = this.gradesForm.get('projectGrade')?.value ?? 0;
    }else{
      this.messageService.add({
        severity: 'info',
        summary: 'Info',
        detail: 'You need to complete both grades'
      });
    }


    this.subscriptions.add = this.gradesService.setStudentGrades(this.examinationSessionId, this.studentGrades)
      .subscribe();
  }
}
