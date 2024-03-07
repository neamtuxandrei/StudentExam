import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminModule } from './admin/admin.module';
import { SecretaryModule } from './secretary/secretary.module';
import { StudentModule } from './student/student.module';
import { CommitteeModule } from './committee/committee.module';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    AdminModule,
    SecretaryModule,
    CommitteeModule,
    StudentModule
  ],
  exports: [
  ]
})
export class FeaturesModule { }
