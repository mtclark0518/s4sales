import { Injectable } from '@angular/core';
// tslint:disable-next-line:import-blacklist
import { Observable, BehaviorSubject } from 'rxjs';
import { CrashEvent } from '../models/crash-event';
import { Cart } from '../models/cart';



@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor() { }

  private Cart = new BehaviorSubject<Cart>(this.getCart());
  public cart = this.Cart.asObservable();

  private handleCart(cart: Cart): void {
    cart.updateCart(cart);
    this.save(cart);
    this.Cart.next(cart);
  }

  public addToCart(item: CrashEvent): void {
    const cart = this.getCart();
      cart.items.push(item);
      this.handleCart(cart);
  }

  public removeFromCart(remove: CrashEvent): void {
    let cart = this.getCart();
    const filtered = cart.items.filter(item => item.hsmv_report_number !== remove.hsmv_report_number );
    this.emptyCart();
    cart = this.getCart();
    filtered.forEach(item => {
      cart.items.push(item);
    });
    console.log(cart);
    this.handleCart(cart);
  }

  private isCart(): boolean {
    if (window.localStorage.getItem('CART')) { return true; } else { return false; }
  }

  public getCart(): Cart {
    const cart = new Cart();
    if (this.isCart()) {
      cart.updateCart(JSON.parse(window.localStorage.getItem('CART')));
    }
    return cart;
  }
  private save(cart: Cart) {
    window.localStorage.setItem('CART', JSON.stringify(cart));
  }

  public emptyCart(): void {
    const cart = new Cart();
    this.save(cart);
    this.Cart.next(cart);
  }

  public restoreCart(): void {
    const cart = this.getCart();
    this.handleCart(cart);
  }



}
