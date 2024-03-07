import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '', children: [
      {
        path: 'examination-session',
        loadChildren: () => import('./examination-session/examination-session.module').then(m => m.ExaminationSessionModule),
      },
      {
        path: '',
        redirectTo: 'examination-session',
        pathMatch: 'full'
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommitteeRoutingModule { }
