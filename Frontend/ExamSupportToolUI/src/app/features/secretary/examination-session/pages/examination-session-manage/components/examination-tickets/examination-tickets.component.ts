import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { ExaminationTicket } from 'src/app/shared/models/examination-ticket';
import { ExaminationTicketService } from 'src/app/shared/services/examination-ticket.service';
import { FormControl, FormGroup,Validators } from '@angular/forms';
import { NgModel } from '@angular/forms';

import {
  ConfirmationService,
  MessageService,
  ConfirmEventType,
} from 'primeng/api';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { ExaminationSessionService } from 'src/app/shared/services/examination-session.service';
import { ExaminationSession } from 'src/app/shared/models/examination-session';

@Component({
  selector: 'app-examination-tickets',
  templateUrl: './examination-tickets.component.html',
  styleUrls: ['./examination-tickets.component.css'],
})
export class ExaminationTicketsComponent {
  addControl: boolean = false;
  editControl: boolean  =false;

  examinationSessionId: string = '';
  selectedTicketId: string = '';
  ticketNo: number = 0;

  examinationSessions: ExaminationSession[] = [];
  subscriptions = new SubscriptionsContainer();

  ticketToDelete :string = "";
  isTicketDialogVisible: boolean = false;
  isImportTicketsDialogVisible : boolean = false;
  selectedSession?: ExaminationSession;

  displayAddDialog():void{
    this.editTicketForm.reset();
    this.addControl = true;
  }

  closeAddDialog():void{
    this.addControl = false;
  }

  displayEditDialog():void{
    this.editTicketForm.reset();
    this.editControl = true;
  }

  closeEditDialog():void{
    this.editControl = false;
  }
  tickets: ExaminationTicket[] = [];


  addTicketForm = new FormGroup({
    ticketNo: new FormControl(0,Validators.required),
    question1: new FormControl('',Validators.required),
    question2: new FormControl('',Validators.required),
    question3: new FormControl('',Validators.required),
    answer1: new FormControl('',Validators.required),
    answer2: new FormControl('',Validators.required),
    answer3: new FormControl('',Validators.required)
  })

  editTicketForm = new FormGroup({
    ticketNo: new FormControl(),
    question1: new FormControl(''),
    question2: new FormControl(''),
    question3: new FormControl(''),
    answer1: new FormControl(''),
    answer2: new FormControl(''),
    answer3: new FormControl('')

  })

  ticketForm = new FormGroup({
    ticketNo: new FormControl(),
    isActive: new FormControl(),
    question1: new FormControl(''),
    question2: new FormControl(''),
    question3: new FormControl(''),
    answer1: new FormControl(''),
    answer2: new FormControl(''),
    answer3: new FormControl(''),
  });

  ticketActiveDropdown = [{ state: true }, { state: false }];

  constructor(
    private examinationTicketService: ExaminationTicketService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private examinationSessionService: ExaminationSessionService
  ) {}

  importTickets() {
    const id = this.activatedRoute.snapshot.parent?.paramMap.get('id');
    if (this.selectedSession && id) {
      const ticketData = {
        importFromSessionId: this.selectedSession.id,
      };
      this.examinationTicketService.importTicketsFromSession(id,ticketData).subscribe({
        complete: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Tickets imported',
          });
          this.loadExaminationTickets();
        },
        error: (result) => {
          this.messageService.add({severity:'error', summary:'Failed', detail: result.error.description});
        }
      })
      this.isImportTicketsDialogVisible = false;
    }
  }
  confirmRemoveDialog(event:Event,id:string) {
    this.ticketToDelete = id;
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this item?',
      icon: 'pi pi-exclamation-triangle',
    })
    event.stopPropagation();
  }

  onRemoveClick(id:string) {
    this.examinationTicketService.removeTicket(id).subscribe({
      complete: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Ticket removed',
        });
        this.loadExaminationTickets();
      },
      error: () => {
        this.messageService.add({severity:'error', summary:'Failed', detail:'Could not removed'});
      }
    })
  }

  ngOnInit() {
    let id = this.activatedRoute.snapshot.parent?.paramMap.get('id');

    if (id) {
      this.examinationSessionId = id;
    }
    this.loadExaminationTickets();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  loadExaminationTickets() {
    const id = this.activatedRoute.snapshot.parent?.paramMap.get('id');
    if (id)
      this.subscriptions.add = this.examinationTicketService
        .getTickets(id)
        .subscribe((response) => {
          this.tickets = response;
        });
  }

  checkStatus(ticket: ExaminationTicket) {
    if (ticket.isActive == true) return true;
    else return false;
  }

  showTicketDialog() {
    this.isTicketDialogVisible = true;
  }
  hideTicketDialog() {
    this.isTicketDialogVisible = false;
  }

  onEditTicketClick(ticket: ExaminationTicket) {
    this.selectedTicketId = ticket.id;
    this.displayEditDialog();

    this.editTicketForm.patchValue({
      ticketNo: ticket.ticketNo,
      question1: ticket.question1,
      question2: ticket.question2,
      question3: ticket.question3,
      answer1: ticket.answer1,
      answer2: ticket.answer2,
      answer3: ticket.answer3,
    });
  }

  public showImportStudentDialog() {
    this.isImportTicketsDialogVisible = true;
  }
  public hideImportStudentDialog() {
    this.isImportTicketsDialogVisible = false;
  }
  onImportClick() {
    this.examinationSessionService.getExaminationSessions().subscribe({
      next: (sessions) => {
        this.examinationSessions = sessions;
      },
      complete: () => {
      },
      error: (error) => {
        this.messageService.add({severity:'error', summary:'Failed', detail:'Error fetching examination sessions'});
      }
    }
    );
    this.showImportStudentDialog();
  }

  updateTicket(){
    const ticketData = {
      question1:this.editTicketForm?.get('question1')?.value,
      question2:this.editTicketForm?.get('question2')?.value,
      question3:this.editTicketForm?.get('question3')?.value,
      answer1:this.editTicketForm?.get('answer1')?.value,
      answer2:this.editTicketForm?.get('answer2')?.value,
      answer3:this.editTicketForm?.get('answer3')?.value
    }
    if(this.editTicketForm.valid){
      this.subscriptions.add = this.examinationTicketService.updateTicket(this.selectedTicketId,ticketData).subscribe({
        complete: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'The ticket was successfully updated',
          });
          this.loadExaminationTickets();
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Could not update the ticket'
          });
        }
      });
    }
    this.closeEditDialog();
    this.editTicketForm.reset();
  }


  onAddTicketClick() {
    const ticketData = {
      ...this.addTicketForm.value,
      examinationSessionId: this.examinationSessionId,
    };

    if(this.addTicketForm.valid)
    {
     this.subscriptions.add = this.examinationTicketService.addTicketToSession(ticketData).subscribe
     ({
      complete: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Ticket added to the session',
        });
        this.loadExaminationTickets();
      },
      error: () => {
        this.messageService.add({severity:'error', summary:'Failed', detail:'Could not add the ticket'});
      }
     });

   }
   this.closeAddDialog();
   this.addTicketForm.reset();
  }

  onEditClick() {}
}
