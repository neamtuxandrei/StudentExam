import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExaminationSessionComponent } from './examination-session.component';
import { PresentationComponent } from './pages/presentation/presentation.component';

const routes: Routes = [
  {
    path: '', component: ExaminationSessionComponent, children: [
      { path: 'presentation', component: PresentationComponent},
      { path: '', redirectTo: 'presentation', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExaminationSessionRoutingModule { }
