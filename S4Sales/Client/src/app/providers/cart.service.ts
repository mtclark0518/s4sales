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
    this.http.get(this.domain + 'init', {responseType: 'text'}).subscribe( response => {
      this.setCart(response);
    });
  }


  public addToCart(item: CrashEvent) {
    let cart_id: string;
    this.cart.subscribe(id => cart_id = id);
    const hsmv = item.hsmv_report_number.toString();
    const body = {cart_id, hsmv};
    console.log(body);
    this.http.post(this.domain + 'add', body)
      .subscribe(response =>
        console.log(response));
  }

  private setCart(cart) {
    this.Cart.next(cart);
  }

  // public removeFromCart(remove: CartItem): void {

  // }

  public emptyCart(): void {}

}
