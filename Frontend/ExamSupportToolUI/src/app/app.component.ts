import { Component } from '@angular/core';
import { EventTypes, OidcSecurityService, PublicEventsService } from 'angular-auth-oidc-client';
import { firstValueFrom } from 'rxjs';
import { SubscriptionsContainer } from './shared/helpers/subscriptions-container';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ExamSupportToolUI';

  private subscriptions = new SubscriptionsContainer();

  constructor(
    public oidcSecurityService: OidcSecurityService,
    private publicEventService: PublicEventsService,
    private router: Router
  ) { }

  async ngOnInit() {
    this.subscriptions.add = this.oidcSecurityService.checkAuth()
      .subscribe(value => {
        if (value.isAuthenticated) {
          // this.router.navigate([`/${value.userData?.role[0].toLowerCase()}`])
        }
      });

    await this.authenticateUser();
  }

  private async authenticateUser() {
    this.subscriptions.add = this.oidcSecurityService.isAuthenticated$.subscribe(async ({ isAuthenticated }) => {
      const eventType = await this.getFiredEvent();
      if (!isAuthenticated && this.isAuthProcessOrLoggingOut(eventType)) {
        this.oidcSecurityService.authorize();
      }
    })
  }

  /**
   * Checks if the user is in authentication process or if the user is trying to loggout.
   * @param eventType The event type.
   * @return {boolean} True if the user is in the auth/loggout process, false otherwise.
   */
  private isAuthProcessOrLoggingOut(eventType: EventTypes): boolean {
    return eventType !== EventTypes.CheckingAuth && eventType !== EventTypes.UserDataChanged;
  }

  /**
   * Gets the fired event type from the angular-auth-oidc-client public events.
   * @throws An error if the event fire fails.
   * @returns {Promise<EventTypes>} A promise with the fired event type.
   */
  private async getFiredEvent(): Promise<EventTypes> {
    try {
      const value = await firstValueFrom(this.publicEventService.registerForEvents());
      const eventType = value.type;
      return eventType;
    } catch (error) {
      console.error("Error fetching fired event:", error);
      throw error;
    }
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

}