import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NavBarComponent } from './nav-bar/nav-bar.component';
import { AuthModule } from 'angular-auth-oidc-client';
import { TabMenuModule } from 'primeng/tabmenu'
import { ButtonModule } from 'primeng/button';



@NgModule({
  declarations: [NavBarComponent
  ],
  imports: [
    CommonModule,
    TabMenuModule,
    ButtonModule,
    AuthModule
  ],
  exports: [NavBarComponent
  ]
})
export class CoreModule { }
