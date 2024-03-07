import { Component, Input, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-countdown-timer',
  templateUrl: './countdown-timer.component.html',
  styleUrls: ['./countdown-timer.component.css']
})
export class CountdownTimerComponent {
  @Input() startDate: Date = new Date();
  @Input() duration: number = 30;
  @Input() updateIntervalms: number = 1000;

  startDateCopy!: Date;
  timerDisplay: string = '';
  interval: any;

  ngOnInit() {
    this.startDateCopy = new Date(this.startDate);
    if (this.startDateCopy.getFullYear() == 1) {
      this.timerDisplay = 'Not started';
    } else {
      this.startTimer();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ('startDate' in changes || 'duration' in changes) {
      this.startDateCopy = new Date(changes['startDate'].currentValue);
    }
    if (this.startDateCopy.getFullYear() == 1) {
      this.timerDisplay = 'Not started';
    } else {
      this.startTimer();
    }
  }

  ngOnDestory() {
    this.clearTimer();
  }

  private startTimer() {
    this.timerDisplay = this.formatRemainingTime(this.duration * 60 * 1000)
    this.interval = setInterval(() => {
      const currentTime = new Date();
      const elapsedTime = currentTime.getTime() - this.startDateCopy.getTime();
      const remainingTime = (this.duration * 60 * 1000) - elapsedTime;
      if (remainingTime <= 0) {
        this.timerDisplay = 'Timer expired';
        this.clearTimer();
      } else {
        this.timerDisplay = this.formatRemainingTime(remainingTime);
      }
    }, this.updateIntervalms)
  }

  formatRemainingTime(milliseconds: number): string {
    const totalSeconds = Math.floor(milliseconds / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
  }

  private clearTimer(): void {
    clearInterval(this.interval);
  }
}
