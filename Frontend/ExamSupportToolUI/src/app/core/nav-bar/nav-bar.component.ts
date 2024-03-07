import { Component } from '@angular/core';
import { OidcSecurityService, UserDataResult } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { SubscriptionsContainer } from 'src/app/shared/helpers/subscriptions-container';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent {
  subscriptions = new SubscriptionsContainer();

  userData$?: Observable<UserDataResult>

  constructor(private oidcSecurityService: OidcSecurityService) { }

  ngOnInit() {
    this.userData$ = this.oidcSecurityService.userData$;
  }

  logout() {
    this.subscriptions.add = this.oidcSecurityService.logoffAndRevokeTokens().subscribe();
  }

  ngOnDestroy() {
    this.subscriptions.dispose();
  }

  hasStudentRole(role: String[]) {
    return role?.includes("Student");
  }

  hasAdminRole(role: String[]) {
    return role?.includes("Admin");
  }

  hasCommitteeRole(role: String[]) {
    return role?.includes("Committee");
  }

  hasSecretaryRole(role: String[]) {
    return role?.includes("Secretary");
  }

}
