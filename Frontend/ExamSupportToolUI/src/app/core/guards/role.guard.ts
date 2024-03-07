import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { OidcSecurityService, UserDataResult } from 'angular-auth-oidc-client';
import { Observable, firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
    const expectedRole = route.data['expectedRole'];
    const userData = await firstValueFrom(this.oidcSecurityService.userData$);
    const currentRole = userData?.userData?.role[0];

    if (currentRole === expectedRole) {
      return true;
    } else if (currentRole === "Admin") {
      return this.router.parseUrl('/admin');
    } else if (currentRole === "Secretary") {
      return this.router.parseUrl('/secretary');
    } else if (currentRole === "Committee") {
      return this.router.parseUrl('/committee');
    } else if (currentRole === "Student") {
      return this.router.parseUrl('/student');
    } else {
      return false;
    }
  }
}
