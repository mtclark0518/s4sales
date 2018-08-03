import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from './account.service';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthGuard implements CanActivate {
  user;
  constructor( private account: AccountService, private http: HttpClient, private router: Router ) {}

  canActivate( next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    console.log('auth guard service canActivate triggered');
    this.account.getCurrentUser();
    this.account.isUser.subscribe( u => this.user = u );
    console.log(this.user);
    // if (this.user === false) { this.router.navigateByUrl('/login'); }

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
