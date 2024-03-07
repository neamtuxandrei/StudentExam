import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-examination-session-student',
  templateUrl: './examination-session.component.html',
  styleUrls: ['./examination-session.component.css']
})
export class ExaminationSessionComponent {
  items: MenuItem[] | undefined;

  activeItem: MenuItem | undefined;

  ngOnInit() {
    this.items = [
      { label: 'Presentation', icon: 'pi pi-fw pi-video', routerLink: 'presentation' },
    ];

    this.activeItem = this.items[0];
  }

  onActiveItemChange(event: MenuItem) {
    this.activeItem = event;
  }
}
