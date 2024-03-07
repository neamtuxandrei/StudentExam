import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InputText, InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

import { ExaminationSessionRoutingModule } from './examination-session-routing.module';
import { ExaminationSessionManageModule } from './pages/examination-session-manage/examination-session-manage.module';

import { ExaminationSessionComponent } from './examination-session.component';
import { ExaminationSessionListComponent } from './pages/examination-session-list/examination-session-list.component';




@NgModule({
  declarations: [
    ExaminationSessionListComponent,
    ExaminationSessionComponent
  ],
  imports: [
    CommonModule,
    ExaminationSessionRoutingModule,
    ExaminationSessionManageModule,
    ButtonModule,
    DialogModule,
    CalendarModule,
    InputTextModule,
    TableModule,
    FormsModule,
    ReactiveFormsModule
  ],
})
export class ExaminationSessionModule { }
