import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ExaminationSessionAddComponent } from './pages/examination-session-add/examination-session-add.component';
import { ExaminationSessionListComponent } from './pages/examination-session-list/examination-session-list.component';
import { ExaminationSessionComponent } from './examination-session.component';


const routes: Routes = [
  {
    path: '', component: ExaminationSessionComponent, children: [
      { path: 'add', component: ExaminationSessionAddComponent },
      { path: 'list', component: ExaminationSessionListComponent },
      { path: ':id', loadChildren: () => import('./pages/examination-session-manage/examination-session-manage.module').then(m => m.ExaminationSessionManageModule) },
      { path: '', redirectTo: 'list', pathMatch: 'full' }
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ], exports: [RouterModule]
})
export class ExaminationSessionRoutingModule { }
