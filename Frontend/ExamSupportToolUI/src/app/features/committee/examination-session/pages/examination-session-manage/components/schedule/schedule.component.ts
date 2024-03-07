import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { findEntryIndexById, groupPresentationScheduleByDate } from 'src/app/shared/helpers/student-helper';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { PresentationSchedule, PresentationScheduleEntry } from 'src/app/shared/models/presentation-schedule';
import { PresentationScheduleService } from 'src/app/shared/services/committee/presentation-schedule.service';
import { SignalRService } from 'src/app/shared/services/signal-r.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent {
  subscriptions = new SubscriptionsContainer();

  activeTabIndex: number = 0;
  examinationSessionId: any;

  presentationSchedule: PresentationSchedule = {} as PresentationSchedule;
  presentationScheduleEntries: PresentationScheduleEntry[] = [];
  presentationScheduleByDate: any[] = []

  constructor(
    private presentationScheduleService: PresentationScheduleService,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private signalRService: SignalRService
  ){}

  ngOnInit() {
    this.loadSessionId();
    this.loadPresentationSchedule();
    this.loadSignalRSubscriptions();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  getEntryIndex(entryId: string) {
    return findEntryIndexById(this.presentationScheduleEntries, entryId);
  }

  private loadSignalRSubscriptions(){
    this.subscriptions.add = this.signalRService.registerForNotification("onScheduleUpdate")
    .subscribe(() => {
      this.loadPresentationSchedule();
    });
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
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Something went wrong' });
        }
      });
  }
}
