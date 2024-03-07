import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, map, Observable, Subscription } from 'rxjs';
import { StudentGrade } from '../../../../../../../shared/models/student-grade';
import { StudentGradesService } from '../../../../../../../shared/services/grading/student-grades.service';
import { SignalRService } from 'src/app/shared/services/signal-r.service';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';


@Component({
  selector: 'app-student-grades',
  templateUrl: './student-grades.component.html',
  styleUrls: ['./student-grades.component.css']
})
export class StudentGradesComponent implements OnInit {
  public studentsListSubscription?: Subscription;
  public studentsList?: StudentGrade[];
  public changedStudentSubscription?: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private studentGradesService: StudentGradesService,
    private signalRService: SignalRService
  ) { }

  ngOnInit(): void {
    this.loadStudentGrades();
    this.initSignalRSubscriptions();
  }

  private loadStudentGrades() {
    let sessionId = this.activatedRoute.snapshot.parent?.paramMap.get('id') ?? '';
    this.studentsListSubscription = this.studentGradesService.getStudentsGrades(sessionId)
      .subscribe(students => {
        this.studentsList = students;
      });
  }

  private initSignalRSubscriptions() {
    this.signalRService.registerForNotification("OnStudentGradeUpdate").subscribe(() => {
      this.loadStudentGrades(); // refresh the list
    });
  }

  onGradeChanged(student: StudentGrade)
  {
    let sessionId = this.activatedRoute.snapshot.parent?.paramMap.get('id') ?? '';
    if (this.changedStudentSubscription !== undefined)
      this.changedStudentSubscription.unsubscribe();
    this.changedStudentSubscription = this.studentGradesService.setStudentGrades(sessionId, student)
      .subscribe(
        (newStudentData: StudentGrade)=>
                                                                    {
          let studentsInList = this.studentsList!
            .filter(stud => stud.id === newStudentData.id);
          if (studentsInList.length > 0)
                                                                      {
            let studentInList = studentsInList.at(0);
            studentInList!.projectAverage = newStudentData.projectAverage;
            studentInList!.theoryAverage = newStudentData.theoryAverage;
            studentInList!.projectGrade = newStudentData.projectGrade;
            studentInList!.theoryGrade = newStudentData.theoryGrade;
          }
        }
      );
  }

  ngOnDestroy()
  {
    if (this.changedStudentSubscription !== undefined)
      this.changedStudentSubscription.unsubscribe();

    if (this.studentsListSubscription !== undefined)
      this.studentsListSubscription.unsubscribe();
  }
}
