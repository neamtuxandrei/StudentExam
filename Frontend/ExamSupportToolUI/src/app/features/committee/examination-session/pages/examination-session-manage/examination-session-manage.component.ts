import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-examination-session-manage-committee',
  templateUrl: './examination-session-manage.component.html',
  styleUrls: ['./examination-session-manage.component.css']
})
export class ExaminationSessionManageComponent {
  items: MenuItem[] | undefined;

  activeItem: MenuItem | undefined;

  ngOnInit() {
    this.items = [
      { label: 'Schedule', icon: 'pi pi-fw pi-calendar', routerLink: 'schedule' },
      { label: 'Presentation', icon: 'pi pi-fw pi-video', routerLink: 'presentation' },
      { label: 'Grades', icon: 'pi pi-fw pi-briefcase', routerLink: 'grades' }
    ];

    this.activeItem = this.items[0];
  }

  onActiveItemChange(event: MenuItem) {
    this.activeItem = event;
}
}
