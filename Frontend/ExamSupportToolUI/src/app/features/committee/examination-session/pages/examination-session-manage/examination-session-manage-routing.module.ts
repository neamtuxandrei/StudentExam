import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ExaminationSessionManageComponent } from './examination-session-manage.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { PresentationComponent } from './components/presentation/presentation.component';
import { GradesComponent } from './components/grades/grades.component';

const routes: Routes = [
  {
    path: '',
    component: ExaminationSessionManageComponent, children: [
      { path: '', redirectTo: 'schedule', pathMatch: 'full' },
      { path: 'schedule', component: ScheduleComponent },
      { path: 'presentation', component: PresentationComponent },
      { path: 'grades', component: GradesComponent },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExaminationSessionManageRoutingModule {}
