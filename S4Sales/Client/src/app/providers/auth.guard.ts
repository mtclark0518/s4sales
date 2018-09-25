import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from './account.service';
import { HttpClient } from '@angular/common/http';
import { OnboardingDetails } from '../models/_classes';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor( private account: AccountService, private http: HttpClient, private router: Router ) {}

  canActivate( next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    this.account.getCurrentUser(); // redirects to login component if false
    if (next.queryParams.error) {
      // handle error
      console.log(next.queryParams.error_description);
      return false;
    }
    // if this is a redirect from stripe onboarding we should have a state string
    if (next.queryParams.state) {
      // double check that we do have an authorization code
      if (next.queryParams.code) {
        const details = new OnboardingDetails();
        details.agency = next.queryParams.state;
        details.token = next.queryParams.code;
        this.account.setOnboardingDetails(details);
        return true;
      }
    }
    return true;
  }
}

@Injectable()
export class AdminGuard implements CanActivate {
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    console.log('admin guard service canActivate triggered');
    return true;
  }
}
