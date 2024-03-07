import { Component, OnInit } from '@angular/core';
import { ConfirmEventType, ConfirmationService, MessageService } from 'primeng/api';
import { Student } from 'src/app/shared/models/student';
import { StudentService } from 'src/app/shared/services/student.service';
import { ActivatedRoute } from '@angular/router';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';
import { FormControl, FormGroup, Validators,AbstractControl } from '@angular/forms';
import { FileUploadEvent } from 'primeng/fileupload';
import { XLSXReader } from 'src/app/shared/helpers/xlsx-reader';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css'],
})
export class StudentsComponent implements OnInit {
  subscriptions = new SubscriptionsContainer();
  studentListSubject = new BehaviorSubject(true);
  studentList$?: Observable<Student[]>;
  studentList: Student[] = [];

  isAddDialogVisible: boolean = false;
  isEditDialogVisible: boolean = false;
  isImportStudentDialogVisible: boolean = false;
  isLoadingStudentsSpinnerVisible: boolean = false;

  selectedStudentId: string = '';
  examinationSessionId: string = '';

  studentForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
    email: new FormControl('', [Validators.required, Validators.email]),
    anonymizationCode: new FormControl('', Validators.required),
    yearsAverageGrade: new FormControl(0, [Validators.required, Validators.pattern('^[0-9]*(\.[0-9]+)?$')
    ,this.validateValue(10, 5)]),
    diplomaProjectName: new FormControl('', Validators.required),
    coordinatorName: new FormControl('', Validators.required),
  });

  constructor(
    private messageService: MessageService,
    private studentService: StudentService,
    private activatedRoute: ActivatedRoute,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
    let id = this.activatedRoute.snapshot.parent?.paramMap.get('id');
    if (id) {
      this.examinationSessionId = id;
    }
    this.loadStudents();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  onAddButtonClick() {
    const studentData = {
      ...this.studentForm.value,
      examinationSessionId: this.examinationSessionId,
    };
    if (this.studentForm.valid) {
      this.subscriptions.add = this.studentService.addStudent(studentData).subscribe({
        complete: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Student added successfully',
          });
          this.refreshStudentList();
        },
        error: (result) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: result.error.description });
        },
      });
    }
    this.hideAddDialog();
    this.studentForm.reset();
  }

   validateValue(maxValue: number, minValue: number) {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const value = parseFloat(control.value);

      if (isNaN(value) || value > maxValue || value < minValue) {
        return { 'maxValue': true };
      }

      return null;
    };
  }

  onCancelButtonClick() {
    this.hideAddDialog();
  }

  onUpdateStudentButtonClick() {
    if (this.studentForm.valid) {
      this.studentService.updateStudent(this.selectedStudentId, this.studentForm.value).subscribe({
        complete: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Student updated successfully' });
          this.refreshStudentList();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Service Message', detail: 'Could not update' });
        },
      });
    }
    this.hideEditDialog();
  }

  onRemoveStudentClick(studentId: string, $event: MouseEvent) {
    $event.stopPropagation();

    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteStudent(studentId);
      },
      reject: (type: ConfirmEventType) => {
      }
    });
  }

  onCancelUpdateStudentButtonClick() {
    this.hideEditDialog();
  }

  onStudentClick(student: Student) {
    this.showEditDialog(student);
  }

  onImportButtonClick() {
    this.showImportStudentDialog();
  }

  onAddStudentButtonClick() {
    this.showAddDialog();
  }

  onUpload($event: FileUploadEvent) {
    let input = $event.files[0];

    this.showLoadingStudentsSpinner();

    if (this.examinationSessionId) {
      XLSXReader.readStudents(input).then(result => {
        this.subscriptions.add = this.studentService.addStudentsBulk(this.examinationSessionId, result).subscribe({
          error: (result) => {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: result.error.description, life: 6000 });
            this.hideLoadingStudentsSpinner();
          },
          complete: () => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Students have been successfully imported.' });
            this.refreshStudentList();
            this.hideLoadingStudentsSpinner();
          }
        })
        this.hideImportStudentDialog();
      });
    }
  }

  private deleteStudent(id: string) {
    this.subscriptions.add = this.studentService.removeStudent(id, this.examinationSessionId).subscribe({
      complete: () => {
        this.refreshStudentList();
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'The student was successfully removed' });
      },
      error: (response) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: response.error.description });
      }
    })
  }

  private loadStudents() {
    if (this.examinationSessionId) {
      this.studentList$ = this.studentListSubject.asObservable().pipe(
        switchMap(() => this.studentService.getStudents(this.examinationSessionId)),
        // we need to save the students so that we can access them in ts component
        // the studentList is updated whenever the observable emits a new value.
        // from subscribe, asyncpipe etc
        tap(result => this.studentList = result)
      );
    }
  }

  private refreshStudentList() {
    this.studentListSubject.next(true);
  }

  private showAddDialog() {
    this.studentForm.reset();
    this.isAddDialogVisible = true;
  }

  private hideAddDialog() {
    this.isAddDialogVisible = false;
  }

  private showEditDialog(student: Student) {
    this.isEditDialogVisible = true;
    this.selectedStudentId = student.id;
    this.studentForm.patchValue(student);
  }

  private hideEditDialog() {
    this.isEditDialogVisible = false;
  }

  private showImportStudentDialog() {
    this.isImportStudentDialogVisible = true;
  }

  private hideImportStudentDialog() {
    this.isImportStudentDialogVisible = false;
  }

  private showLoadingStudentsSpinner() {
    this.isLoadingStudentsSpinnerVisible = true;
    this.messageService.add({ severity: 'info', summary: 'Importing students', detail: 'Please wait.' });
  }

  private hideLoadingStudentsSpinner() {
    this.isLoadingStudentsSpinnerVisible = false;
  }
}
