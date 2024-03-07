import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommitteeRoutingModule } from './committee-routing.module';
import { ExaminationSessionModule } from './examination-session/examination-session.module';




@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,
    CommitteeRoutingModule,
    ExaminationSessionModule    
  ]
})
export class CommitteeModule { }
