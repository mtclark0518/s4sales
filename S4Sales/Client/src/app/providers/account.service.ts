import { Injectable } from '@angular/core';
import { BehaviorSubject, AsyncSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AccountProfile, Credentials, S4Request } from '../models/_class';
import { Password } from '../models/_interface';


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
  private CurrentAccount = new AsyncSubject<AccountProfile>();
  public currentAccount = this.CurrentAccount.asObservable();

  private AccountStatus = new BehaviorSubject<string>('');
  public accountStatus = this.AccountStatus.asObservable();

  public setAccount($event) {this.CurrentAccount.next($event); }
  public changeStatus($event) { this.AccountStatus.next($event); }

  constructor( private http: HttpClient, private router: Router ) { }

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

  public register(account: S4Request) {
    this.http.post(`${this.domain}/identity/register`, account)
    .subscribe(response => { this.handleRegister(response); });
  }

  // methods to be used by auth guard to verify current user
  public getCurrentUser = (): void => {
    this.http.get(this.domain + '/identity/current').subscribe(response =>
      this.handleCurrentUser(response));
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

  // submits a request for account
  // redirects to the handler component
  private handleRegister (data) {
    console.log(data);
    this.router.navigateByUrl('/request');
  }

  // TODO
  public changePassword(pass: Password) {}
  public resetPassword(pass: Password) {}
  public recoverAccount(email) {}
}

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
