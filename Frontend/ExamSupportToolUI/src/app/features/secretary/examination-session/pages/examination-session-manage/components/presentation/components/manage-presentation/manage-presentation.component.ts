import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, firstValueFrom, generate, of, switchMap, tap } from 'rxjs';
import { groupPresentationScheduleByDate, moveEntry, findEntryIndexById } from 'src/app/shared/helpers/student-helper';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { PresentationSchedule, PresentationScheduleEntry } from 'src/app/shared/models/presentation-schedule';
import { PresentationScheduleService } from 'src/app/shared/services/presentation-schedule.service';

@Component({
  selector: 'app-manage-presentation',
  templateUrl: './manage-presentation.component.html',
  styleUrls: ['./manage-presentation.component.css']
})
export class ManagePresentationComponent {
  subscriptions = new SubscriptionsContainer();

  activeTabIndex: number = 0;
  examinationSessionId: any;

  presentationSchedule: PresentationSchedule = {} as PresentationSchedule;
  presentationScheduleEntries: PresentationScheduleEntry[] = [];
  presentationScheduleByDate: any[] = [] // the values that we use in our tabview and tables

  generateDialogControl: boolean = false;

  presentationScheduleForm = new FormGroup({
    startDate: new FormControl<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
    endDate: new FormControl<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
    breakStart: new FormControl<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
    breakDuration: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required] }),
    studentPresentationDuration: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required] })
  });

  constructor(
    private presentationScheduleService: PresentationScheduleService,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.loadSessionId();
    this.loadPresentationSchedule();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  displayGenerateDialog(): void {
    this.generateDialogControl = true;
    this.patchFormValues();
  }

  closeGenerateDialog(): void {
    this.generateDialogControl = false;
  }


  async onUpButtonClick(entryId: string) {
    this.presentationScheduleByDate = moveEntry(this.presentationScheduleEntries, entryId, 'up');
    await firstValueFrom(this.presentationScheduleService.movePresentationScheduleEntry(this.examinationSessionId, { entryId, direction: 'up' }))
  }

  async onDownButtonClick(entryId: string) {
    this.presentationScheduleByDate = moveEntry(this.presentationScheduleEntries, entryId, 'down');
    await firstValueFrom(this.presentationScheduleService.movePresentationScheduleEntry(this.examinationSessionId, { entryId, direction: 'down' }));
  }

  async onSubmitButtonClick() {
    this.closeGenerateDialog();
    await firstValueFrom(this.presentationScheduleService.generatePresentationSchedule(this.examinationSessionId, { ...this.getFormValuesWithLocalHourFormat() }))
    this.loadPresentationSchedule();
  }

  getEntryIndex(entryId: string) {
    return findEntryIndexById(this.presentationScheduleEntries, entryId);
  }

  private loadSessionId() {
    let id = this.activatedRoute.snapshot.parent?.parent?.paramMap.get('id');

    if (id) {
      this.examinationSessionId = id;
    }
  }

  private loadPresentationSchedule() {
    this.subscriptions.add = this.presentationScheduleService.getPresentationSchedule(this.examinationSessionId)
      .subscribe({
        next: result => {
          this.presentationSchedule = result;
          this.presentationScheduleEntries = this.presentationSchedule.presentationScheduleEntries;
          this.presentationScheduleByDate = groupPresentationScheduleByDate(this.presentationScheduleEntries);
        },
        error: () =>{
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'You need at least one student to generate a schedule' });
        }
      });
  }

  private patchFormValues() {
    let startDate = new Date(this.presentationSchedule.startDate)
    let endDate = new Date(this.presentationSchedule.endDate);
    let breakStart = new Date(this.presentationSchedule.breakStart);

    this.presentationScheduleForm.controls['startDate'].setValue(startDate);
    this.presentationScheduleForm.controls['endDate'].setValue(endDate);
    this.presentationScheduleForm.controls['breakStart'].setValue(breakStart);
    this.presentationScheduleForm.controls['breakDuration'].setValue(this.presentationSchedule.breakDuration);
    this.presentationScheduleForm.controls['studentPresentationDuration'].setValue(this.presentationSchedule.studentPresentationDuration);
  }

  private getFormValuesWithLocalHourFormat() {
    let presentationSchedule = {
      startDate: this.isoToEEST(this.presentationScheduleForm.controls['startDate'].value),
      endDate: this.isoToEEST(this.presentationScheduleForm.controls['endDate'].value),
      breakStart: this.isoToEEST(this.presentationScheduleForm.controls['breakStart'].value),
      breakDuration: this.presentationScheduleForm.controls['breakDuration'].value,
      studentPresentationDuration: this.presentationScheduleForm.controls['studentPresentationDuration'].value
    };
    return presentationSchedule;
  }

  private isoToEEST(date: Date) {
    date.setMinutes(date.getMinutes() + 3 * 60);
    return date.toISOString();
  }

  onSubmitScheduleClick(){
    this.router.navigate(['../' + "start"], { relativeTo: this.activatedRoute });
  }
}
