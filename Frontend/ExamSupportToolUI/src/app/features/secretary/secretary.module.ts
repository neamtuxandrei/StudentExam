import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SecretaryRoutingModule } from './secretary-routing.module';
import { ExaminationSessionModule } from './examination-session/examination-session.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SecretaryRoutingModule,
    ExaminationSessionModule
  ]
})
export class SecretaryModule { }
