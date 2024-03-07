import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-examination-session-manage',
  templateUrl: './examination-session-manage.component.html',
  styleUrls: ['./examination-session-manage.component.css']
})
export class ExaminationSessionManageComponent {
  items: MenuItem[] | undefined;

  activeItem: MenuItem | undefined;

  ngOnInit() {
    this.items = [
      { label: 'Students', icon: 'pi pi-fw pi-user', routerLink: 'students' },
      { label: 'Examination Tickets', icon: 'pi pi-fw pi-list', routerLink: 'examination-tickets' },
      { label: 'Committee', icon: 'pi pi-fw pi-users', routerLink: 'committee' },
      { label: 'Presentation', icon: 'pi pi-fw pi-video', routerLink: 'presentation' },
      { label: 'Examination Reports', icon: 'pi pi-fw pi-chart-bar', routerLink: 'examination-reports' }
    ];

    this.activeItem = this.items[0];
  }

  onActiveItemChange(event: MenuItem) {
    this.activeItem = event;
}
}
