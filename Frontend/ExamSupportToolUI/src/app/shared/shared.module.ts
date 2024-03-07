import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { HttpClientModule } from '@angular/common/http';
import { CountdownTimerComponent } from './pages/countdown-timer/countdown-timer.component';


@NgModule({
  declarations: [
    NotFoundComponent,
    CountdownTimerComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports: [CountdownTimerComponent]
})
export class SharedModule { }
