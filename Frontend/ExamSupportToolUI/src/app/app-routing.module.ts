import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './shared/pages/not-found/not-found.component';
import { RoleGuard } from './core/guards/role.guard';


const routes: Routes = [
  {
    path: '', component: NotFoundComponent, canActivate: [RoleGuard], data: {expectedRole: ''}
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule),
    canActivate: [RoleGuard],
    data: { expectedRole: 'Admin' }
  },
  {
    path: 'secretary',
    loadChildren: () => import('./features/secretary/secretary.module').then(m => m.SecretaryModule),
    canActivate: [RoleGuard],
    data: { expectedRole: 'Secretary' }
  },
  {
    path: 'committee',
    loadChildren: () => import('./features/committee/committee.module').then(m => m.CommitteeModule),
    canActivate: [RoleGuard],
    data: { expectedRole: 'Committee' }
  },
  {
    path: 'student',
    loadChildren: () => import('./features/student/student.module').then(m => m.StudentModule),
    canActivate: [RoleGuard],
    data: { expectedRole: 'Student' }
  },
  {
    path: '**', component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
