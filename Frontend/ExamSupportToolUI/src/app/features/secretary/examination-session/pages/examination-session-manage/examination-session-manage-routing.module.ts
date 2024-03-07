import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ExaminationSessionManageComponent } from './examination-session-manage.component';
import { StudentsComponent } from './components/students/students.component';
import { ExaminationTicketsComponent } from './components/examination-tickets/examination-tickets.component';
import { CommitteeComponent } from './components/committee/committee.component';
import { PresentationComponent } from './components/presentation/presentation.component';
import { ExaminationReportsComponent } from './components/examination-reports/examination-reports.component';
import { StartPresentationComponent } from './components/presentation/components/start-presentation/start-presentation.component';
import { ManagePresentationComponent } from './components/presentation/components/manage-presentation/manage-presentation.component';
import { ClosePresentationComponent } from './components/presentation/components/close-presentation/close-presentation.component';

const routes: Routes = [
  {
    path: '', component: ExaminationSessionManageComponent, children: [
      { path: '', redirectTo: 'students', pathMatch: 'full' },
      { path: 'students', component: StudentsComponent },
      { path: 'examination-tickets', component: ExaminationTicketsComponent},
      { path: 'committee', component:CommitteeComponent},
      { path: 'presentation', component: PresentationComponent,children: [
        {path: 'start', component: StartPresentationComponent},
        {path: 'manage', component: ManagePresentationComponent},
        {path: 'close', component:ClosePresentationComponent},
        {path: '', redirectTo: 'start',pathMatch: 'full'}
      ]},
      { path: 'examination-reports', component: ExaminationReportsComponent}
    ]
  },
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ], exports: [RouterModule]
})
export class ExaminationSessionManageRoutingModule { }
