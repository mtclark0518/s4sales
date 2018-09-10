import { Injectable } from '@angular/core';
import { BehaviorSubject, AsyncSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AgencyAccount, Credentials, NewAgency, OnboardingDetails } from '../models/_classes';
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
  domain = 'http://localhost:5000/api';

  headers: HttpHeaders = new HttpHeaders({
    'Accept': 'application/json',
    'Content-Type': 'application/json',
  });

  private Authenticated = new BehaviorSubject<boolean>(false);
  public isUser = this.Authenticated.asObservable();

  private serverChecked = false;
  private CurrentAccount = new AsyncSubject<AgencyAccount>();
  public currentAccount = this.CurrentAccount.asObservable();

  private AccountStatus = new BehaviorSubject<string>('');
  public accountStatus = this.AccountStatus.asObservable();

  public setAccount($event) {this.CurrentAccount.next($event); }
  public changeStatus($event) { this.AccountStatus.next($event); }

  constructor( private http: HttpClient, private router: Router ) { }

  // method to be used by auth guard to verify current user
  public getCurrentUser = (): void => {
    this.http.get(this.domain + '/identity/current').subscribe(response =>
      this.handleCurrentUser(response));
  }
  public login(account: Credentials) {
    return this.http.post(`${this.domain}/identity/login`, account).subscribe(response => {
      this.handleLogin(response);
    });
  }

  public logout(): void {
    this.http.post(`${this.domain}/identity/logout`, {}).subscribe(response => {
      this.CurrentAccount.next({});
    });
  }

  public register(account: NewAgency) {
    this.http.post(`${this.domain}/identity/register`, account)
    .subscribe(response => { this.handleRegister(response); });
  }
  public onboard(account: OnboardingDetails) {
    this.http.put(`${this.domain}/identity/activate`, account)
    .subscribe(response =>  this.handleOnboard(response));
  }



  private handleCurrentUser(data): void {
    console.log(data);
    this.Authenticated.next(data.user);
    console.log(this.Authenticated);
  }

  private handleLogin(data) {
    this.getCurrentUser();
    this.router.navigateByUrl('/account/dashboard');
  }

  private handleRegister (data) {
    console.log(data);
    // if success send the user to the profile page
    // else
    // display the failure msg
  }
  private handleOnboard (data) {
    console.log(data);
    if (data) { } else { }
  }

  // TODO
  public changePassword(pass: Password) {}
  public resetPassword(pass: Password) {}
  public recoverAccount(email) {}
}

// to onboard accounts with stripe
// https://connect.stripe.com/oauth/authorize?response_type=code
// &client_id=ca_DK4LfgjwY5CxXBlZetcDNriX0eW0Zs2M&scope=read_write&state=******


  // Needs backend counterpart
  // public checkServerSession(): Observable<boolean> {
  //   return this.http
  //     .get('api/identity/current-user')
  //     .map( response => {
  //       console.log(response);
  //       const user = response as Account;
  //       this.serverChecked = true;
  //       this.account = user;
  //       return true;
  //     });
  // }
