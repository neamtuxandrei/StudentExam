import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExaminationSessionManageRoutingModule } from './examination-session-manage-routing.module';
import { ExaminationSessionManageComponent } from './examination-session-manage.component';
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
import { TabViewModule } from 'primeng/tabview';

import { CalendarModule } from 'primeng/calendar';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { PresentationComponent } from './components/presentation/presentation.component';
import { GradesComponent } from './components/grades/grades.component';
import { StudentGradesComponent } from './components/student-grades/student-grades.component';
import { SharedModule } from "../../../../../shared/shared.module";


@NgModule({
    declarations: [ExaminationSessionManageComponent, ScheduleComponent, PresentationComponent, GradesComponent, StudentGradesComponent],
    imports: [
        CommonModule,
        ExaminationSessionManageRoutingModule,
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
        SharedModule
    ]
})
export class ExaminationSessionManageModule { }
