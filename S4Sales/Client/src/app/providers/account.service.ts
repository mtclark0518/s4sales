import { Injectable } from '@angular/core';
import { BehaviorSubject, AsyncSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Credentials, NewAgency, OnboardingDetails, AgencyAccount } from '../models/_classes';
import { Password } from '../models/_interfaces';

export class UserCheck {
  user: boolean;
  name?: string;
  roles?: Array<string>;
}
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private domain = 'http://localhost:5000/api/identity';

  private headers: HttpHeaders = new HttpHeaders({
    'Accept': 'application/json',
    'Content-Type': 'application/json'
  });

  private Authenticated = new BehaviorSubject<boolean>(false);
  public isUser = this.Authenticated.asObservable();

  private AccountStatus = new BehaviorSubject<string>('');
  public accountStatus = this.AccountStatus.asObservable();

  private AgencyDetails = new BehaviorSubject<AgencyAccount>(null);
  public details = this.AgencyDetails.asObservable();

  private CurrentAccount = new BehaviorSubject<string>('');
  public currentAccount = this.CurrentAccount.asObservable();

  private Onboarding = new BehaviorSubject<OnboardingDetails>(null);
  public onboardingDetails = this.Onboarding.asObservable();

  public setAccountStatus(value: string) {this.AccountStatus.next(value); }
  public setAccount(value: string) {this.CurrentAccount.next(value); }
  public setAgencyDetails(value: AgencyAccount) {this.AgencyDetails.next(value); }
  public setOnboardingDetails(value: OnboardingDetails) {this.Onboarding.next(value); }

  constructor( private http: HttpClient, private router: Router ) { }

  // method to be used by auth guard to verify current user
  public getCurrentUser = (): void => {
    this.http.get(this.domain + '/current')
      .subscribe( response => this.handleCurrentUser(response));
  }

  private getProfile = (id: string) => {
    this.headers['agency'] = id;
    this.http.get(`${this.domain}/details`, {headers: this.headers})
      .subscribe( response => this.handleProfile(response));
  }

  public login(account: Credentials) {
    return this.http.post(`${this.domain}/login`, account)
    .subscribe( response => this.handleEntry(response));
  }

  public logout(): void {
    this.http.post(`${this.domain}/logout`, {}).subscribe( () => {
      this.CurrentAccount.next('');
    });
  }

  public onboard(agency: OnboardingDetails) {
    this.http.put(this.domain + '/activate', agency)
    .subscribe( response => this.handleOnboard(response));
  }

  public register(account: NewAgency) {
    this.http.post(this.domain + '/register', account)
    .subscribe( response => this.handleEntry(response));
  }

  private handleCurrentUser(current): void {
    this.Authenticated.next(current.user);
    if (current.user) {
      this.setAccount(current.name);
      this.setAgencyDetails(current.details);
    } else {
      this.router.navigateByUrl('/login');
    }
  }

  private handleEntry (attempt) {
    // if success send the user to the profile page
    if (attempt.succeeded) {
      this.router.navigateByUrl('/account');
    } else {
      // TODO
      // display the failure msg
    }
  }

  private handleProfile(details) {
    // TODO
  }

  private handleOnboard (data) {
    if (data) {
      // TODO
    } else {
      // TODO
    }
  }

  public changePassword(pass: Password) {
    // TODO
  }
  public resetPassword(pass: Password) {
    // TODO
   }

  public recoverAccount(email) {
    this.http.post(this.domain + '/recover', email)
    .subscribe(response => this.handleRecoverAccount(response));
  }

  private handleRecoverAccount(data) {
    if (data.message === 'sent') {
      // TODO
    } else {
      // TODO
    }
  }
}
