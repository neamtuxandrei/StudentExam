import { Component } from '@angular/core';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { ExaminationSession } from 'src/app/shared/models/examination-session';
import { ExaminationSessionService } from 'src/app/shared/services/examination-session.service';
import { SessionStatus } from 'src/app/shared/models/examination-session-status';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { CommitteeService } from 'src/app/shared/services/committee.service';


@Component({
  selector: 'app-examination-session-list',
  templateUrl: './examination-session-list.component.html',
  styleUrls: ['./examination-session-list.component.css'],
})
export class ExaminationSessionListComponent {
  subscriptions = new SubscriptionsContainer();
  examinationSessionListSubject = new BehaviorSubject(true);
  examinationSessionList$?: Observable<ExaminationSession[]>;
  examinationSessionList: ExaminationSession[] = [];
  selectedExaminationSessionCommittees?: any;

  sessionStatusType = SessionStatus;
  minSelectableDate: Date = new Date();

  isCreateSessionDialogVisible: boolean = false;
  isImportDialogVisible: boolean = false;
  isSessionMembersDialogVisible: boolean = false;

  committeeMembers: { name: string; email: string }[] = [];

  examSessionForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    startDate: new FormControl('',[Validators.required]),
    endDate: new FormControl('',[Validators.required]),
  });
  newCommitteeMemberForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
    email: new FormControl('', [Validators.required, Validators.email])
  });


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

  onSubmitSessionClick(): void {
    const sessionData = {
      ...this.examSessionForm.value,
      committeeMembers: this.committeeMembers,
    };
    if (this.examSessionForm.valid) {
      this.subscriptions.add = this.examinationSessionService.addExaminationSession(sessionData).subscribe({
        complete: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Examination session created',
          });
          this.refreshExaminationSessionsList();
          this.examSessionForm.reset();
          this.committeeMembers = [];
        },
        error: (result) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: result.error.description });
        },
      });
    this.closeCreateSessionDialog();
    }else{
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Fields are not valid'});
    }
  }

  private loadExaminationSessions() {
    this.examinationSessionList$ = this.examinationSessionListSubject.asObservable().pipe(
      switchMap(() => this.examinationSessionService.getExaminationSessions()),
      tap(result => this.examinationSessionList = result)
    );
  }

  private loadCommitteeMembers(id: string) {
      this.subscriptions.add = this.committeeService.getCommitteeMembers(id).subscribe( committees =>{
        this.selectedExaminationSessionCommittees = committees;
      })
  }

  private refreshExaminationSessionsList() {
  this.examinationSessionListSubject.next(true);
  }

  addCommitteeMember(): void {
    if (this.newCommitteeMemberForm.valid) {
      const nameValue = this.newCommitteeMemberForm.get('name')?.value;
      const emailValue = this.newCommitteeMemberForm.get('email')?.value;

      if(nameValue && emailValue)
      {
        let emailExists = this.committeeMembers.some(
          (member) => member.email === emailValue);
        if(!emailExists)
        {
          this.committeeMembers.push({ name: nameValue, email: emailValue })
        }else{
          this.messageService.add({
           severity: 'error',
           summary: 'Error',
           detail: 'A committee with this email already exists'});
        }
      }

      this.newCommitteeMemberForm.reset();
    }else{
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Fields are not valid'});
    }
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

onImportCommitteesClick(){
 if(this.selectedExaminationSessionCommittees <= 0){
  this.messageService.add({
    severity: 'error',
    summary: 'Error',
    detail: 'There are no committees to import',
  });
 }else {
  let atLeastOneNotAdded= true;

   for (let committee of this.selectedExaminationSessionCommittees) {
        const emailExists = this.committeeMembers.some((member)=> member.email === committee.email);
        if(!emailExists){
          this.committeeMembers.push(committee);
          atLeastOneNotAdded = false;
        }
    }

    if (atLeastOneNotAdded) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Email must be unique',
      });
    } else {
      this.messageService.add({
        severity: 'success',
        summary: 'Success',
        detail: 'Committees imported',
      });
    }
  }

  this.closeImportMembersDialog();
  this.closeSessionMembersDialog();
 }

 onRemoveCommitteeFromNewSessionClick(name:string,email :string){
  this.committeeMembers = this.committeeMembers.filter(member => member.name !== name && member.email !== email );
 }

  onCloseSessionClick(id: string, event: Event)
  {
    this.subscriptions.add = this.examinationSessionService.setExaminationSessionStatus(id, 3).subscribe({
      complete: () => {
        this.loadExaminationSessions();
      },
    });
    event.stopPropagation();
  }

  onOpenSessionClick(id: string, event: Event)
  {
    this.subscriptions.add = this.examinationSessionService.setExaminationSessionStatus(id, 1).subscribe({
      complete: () => {
        this.loadExaminationSessions();
      },
    });
    event.stopPropagation();
  }

  onSessionClick(id: string) {
    this.router.navigate(['../' + id], { relativeTo: this.route });
  }

  onCreateSessionClick()
  {
    this.displayCreateSessionDialog();
  }

  displayCreateSessionDialog() {
    this.isCreateSessionDialogVisible = true;
  }

  closeCreateSessionDialog(){
    this.isCreateSessionDialogVisible = false;
  }

  displaySessionMembersDialog(session : ExaminationSession) {
    this.loadCommitteeMembers(session.id);
    this.isSessionMembersDialogVisible = true;
  }

  closeSessionMembersDialog() {
    this.isSessionMembersDialogVisible = false;
  }

  displayImportMembersDialog() {
    this.isImportDialogVisible = true;
  }

  closeImportMembersDialog() {
    this.isImportDialogVisible = false;
  }
}
