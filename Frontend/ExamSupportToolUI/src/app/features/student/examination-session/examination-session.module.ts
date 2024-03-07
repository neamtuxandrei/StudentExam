import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExaminationSessionRoutingModule } from './examination-session-routing.module';
import { ExaminationSessionComponent } from './examination-session.component';
import { ButtonModule } from 'primeng/button';
import { SharedModule } from 'src/app/shared/shared.module';

import { TabMenuModule } from 'primeng/tabmenu';
import { PresentationComponent } from './pages/presentation/presentation.component';

@NgModule({
  declarations: [
    ExaminationSessionComponent,
    PresentationComponent
  ],
  imports: [
    CommonModule,
    ExaminationSessionRoutingModule,
    SharedModule,
    ButtonModule,
    TabMenuModule
  ]
})
export class ExaminationSessionModule { }
