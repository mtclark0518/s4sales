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
    const onboarding = this.stripeGuard(next);
    if (onboarding) { return true; }
    return true; // hard coding for dev
  }

  private stripeGuard(next: ActivatedRouteSnapshot): boolean {
    // stripe onboarding we should have a state string and auth code
    if (next.queryParams.state && next.queryParams.code) {
      const details = new OnboardingDetails();
      details.agency = next.queryParams.state;
      details.token = next.queryParams.code;
      this.account.setOnboardingDetails(details);
      return true;
    }
    if (next.queryParams.error) {
      return false; // TODO add stripe error logic
    }
  }
}

