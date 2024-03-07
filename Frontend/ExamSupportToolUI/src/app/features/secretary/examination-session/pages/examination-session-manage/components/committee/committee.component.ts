import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { CommitteeMember } from 'src/app/shared/models/committee-member';
import { CommitteeService } from 'src/app/shared/services/committee.service';

@Component({
  selector: 'app-committee',
  templateUrl: './committee.component.html',
  styleUrls: ['./committee.component.css']
})
export class CommitteeComponent {

  headOfCommittee? : CommitteeMember
  committeeList?: CommitteeMember[];

  subscriptions = new SubscriptionsContainer();

  isAddDialogVisible: boolean = false;
  isEditDialogVisible: boolean = false;
  isRemoveDialogVisible: boolean = false;

  committeeForm = new FormGroup({
    name: new FormControl(),
    email: new FormControl(),
    isHeadOfCommittee: new FormControl()
  });

  committeeHeadDropdown = [
    { name: 'No', value: false },
    { name: 'Yes', value: true }
  ];

  examinationSessionId: string = '';
  selectedCommitteeId: string = '';

  constructor(
    private committeeService: CommitteeService,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    let id = this.activatedRoute.snapshot.parent?.paramMap.get('id');

    if (id) {
      this.examinationSessionId = id;
    }
    this.loadCommiteeMembers();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  loadCommiteeMembers(): void {
    const id = this.activatedRoute.snapshot.parent?.paramMap.get("id");
    if (id) {
      this.subscriptions.add = this.committeeService.getHeadOfCommitteeBySessionId(id).subscribe(committeeHead => {
              this.headOfCommittee = committeeHead;
            });
      this.subscriptions.add = this.committeeService.getCommitteeMembers(id).subscribe(committees=> {
        this.committeeList = committees
      })
    }
  }

  isHeadOfCommittee(committee:CommitteeMember){
    return committee.id === this.headOfCommittee?.id;
  }

  onAddMemberButtonClick() {
    this.showAddDialog();
  }

  onSubmitButtonClick() {
    const committeeMemberData = {
      ...this.committeeForm.value,
      examinationSessionId: this.examinationSessionId
    };

    if (this.committeeForm.valid) {
      this.subscriptions.add = this.committeeService.addCommitteeMember(committeeMemberData).subscribe({
        complete: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Committee member added successfully',
          });
        this.loadCommiteeMembers();
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Could not add committee member'
          });
        }
      });
    }
    this.hideAddDialog();
    this.committeeForm.reset();
  }

  onImportButtonClick() {
  }

  onEditMemberButtonClick(committee: CommitteeMember) {
    this.selectedCommitteeId = committee.id;
    this.showEditDialog();
    this.committeeForm.patchValue({
      name: committee.name,
      email: committee.email,
      isHeadOfCommittee: this.isHeadOfCommittee(committee),
    });
  }
  
  onRemoveMemberButtonClick(memberId: string,$event: MouseEvent){
    $event.stopPropagation();
    this.selectedCommitteeId = memberId;
    this.displayRemoveDialog();
  }

  onUpdateMemberButtonClick() {
    const committeeData = {
      ...this.committeeForm.value,
      examinationSessionId: this.examinationSessionId,
    };

    if(this.committeeForm.valid){
      this.subscriptions.add = this.committeeService.updateCommittee(this.selectedCommitteeId, committeeData).subscribe({
        complete: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Committee member updated successfully' });
          this.loadCommiteeMembers();
        },
        error: (result) => {
          this.messageService.add({ severity: 'error', summary: 'Service Message', detail: result.error.description });
        }
      });
    }
    this.hideEditDialog();
    this.committeeForm.reset();
  }

  onSubmitRemoveMemberButtonClick() {
    this.committeeService.removeCommitteeMemberFromSession(this.selectedCommitteeId, this.examinationSessionId).subscribe({
      complete: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Committee member delete successfully',
        });
        this.loadCommiteeMembers();
      },
      error: (result) => {
        this.messageService.add({
        severity: 'error', 
        summary: 'Error', 
        detail: 'Could not delete committee member' }); 
      }
  });
  this.closeRemoveDialog();
}

  
  onCancelAddButtonClick() {
    this.hideAddDialog();
  }

  onCancelEditButtonClick() {
    this.hideEditDialog();
  }

  private hideEditDialog() {
    this.isEditDialogVisible = false;
  }

  private showEditDialog() {
    this.committeeForm.reset();
    this.isEditDialogVisible = true;
  }

  private hideAddDialog() {
    this.isAddDialogVisible = false;
  }

  private showAddDialog() {
    this.committeeForm.reset();
    this.isAddDialogVisible = true;
  }

  closeRemoveDialog(){
    this.isRemoveDialogVisible = false;
  }
  private displayRemoveDialog(){
    this.isRemoveDialogVisible = true;
  }
}
