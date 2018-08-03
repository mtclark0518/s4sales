import { Injectable } from '@angular/core';
// tslint:disable-next-line:import-blacklist
import { Observable, BehaviorSubject } from 'rxjs';
import { CrashEvent } from '../models/crash-event';
import { HttpClient } from '@angular/common/http';



@Injectable({
  providedIn: 'root'
})
export class CartService {
  private domain = 'http://localhost:5000/api/cart/';
  constructor(private http: HttpClient) { }

  private Cart = new BehaviorSubject<string>(null);
  public cart = this.Cart.asObservable();
  private Active = new BehaviorSubject<boolean>(false);
  public isActive = this.Active.asObservable();

  public initializeCart() {
    this.http.get(this.domain + 'init').subscribe( cart => {
      this.setCart(cart);
    });
  }
  public addToCart(item: CrashEvent): void {
    let cart_id: string;
    this.cart.subscribe(id => cart_id = id);
    const hsmv = item.hsmv_report_number.toString();
    this.http.post(this.domain + 'cart/add', {cart_id, hsmv})
      .subscribe(response =>
        console.log(response));
  }

  private setCart(response) {
    const cart = response.cart_id;
    this.Cart.next(cart);
  }

  // public removeFromCart(remove: CartItem): void {

  // }

  public emptyCart(): void {}

}
