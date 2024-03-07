import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExaminationSessionManageRoutingModule } from './examination-session-manage-routing.module';
import { ExaminationSessionManageComponent } from './examination-session-manage.component';
import { StudentsComponent } from './components/students/students.component';
import { ExaminationTicketsComponent } from './components/examination-tickets/examination-tickets.component';
import { CommitteeComponent } from './components/committee/committee.component';
import { PresentationComponent } from './components/presentation/presentation.component';
import { ManagePresentationComponent } from './components/presentation/components/manage-presentation/manage-presentation.component';
import { ExaminationReportsComponent } from './components/examination-reports/examination-reports.component';
import { CoreModule } from 'src/app/core/core.module';

import { TabMenuModule } from 'primeng/tabmenu';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { SlideMenuModule } from 'primeng/slidemenu';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';
import { InplaceModule } from 'primeng/inplace';
import { FileUploadModule } from 'primeng/fileupload';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { StartPresentationComponent } from './components/presentation/components/start-presentation/start-presentation.component';
import { TabViewModule } from 'primeng/tabview';

import { ClosePresentationComponent } from './components/presentation/components/close-presentation/close-presentation.component';
import { CalendarModule } from 'primeng/calendar';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [ExaminationSessionManageComponent, StudentsComponent, ExaminationTicketsComponent, CommitteeComponent, PresentationComponent, ExaminationReportsComponent, StartPresentationComponent,ManagePresentationComponent, ClosePresentationComponent],
  imports: [
    CommonModule,
    ExaminationSessionManageRoutingModule,
    SharedModule,
    TabMenuModule,
    CoreModule,
    ButtonModule,
    TableModule,
    SlideMenuModule,
    InputTextModule,
    CheckboxModule,
    RadioButtonModule,
    FormsModule,
    ReactiveFormsModule,
    TooltipModule,
    DialogModule,
    InplaceModule,
    DialogModule,
    FileUploadModule,
    ProgressSpinnerModule,
    InputNumberModule,
    InputTextareaModule,
    DropdownModule,
    TagModule,
    ConfirmDialogModule,
    TabViewModule,
    CalendarModule,
  ]
})
export class ExaminationSessionManageModule { }
