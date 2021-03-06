import { Injectable } from '@angular/core';
import {  BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CrashReport } from '../models/_interfaces';


@Injectable({
  providedIn: 'root'
})
export class CartService {
  // VARIABLES
  private domain = 'http://localhost:5000/api/cart/';
  constructor(private http: HttpClient) {
   }


  // RXJS BEHAVIOR SUBJECTS
  private Cart = new BehaviorSubject<string>(null);
  public cart = this.Cart.asObservable();

  private Active = new BehaviorSubject<boolean>(false);
  public isActive = this.Active.asObservable();

  private ItemCount = new BehaviorSubject<number>(0);
  public cartItems = this.ItemCount.asObservable();

  private Items = new BehaviorSubject<Array<CrashReport>>([]);
  public _items = this.Items.asObservable();

  // PUBLIC METHODS
  public addToCart(item: CrashReport): void {
    const hsmv = item.hsmv_report_number.toString();
    let cart_id: string;
    this.cart.subscribe(id => cart_id = id);
    const body = {cart_id, hsmv};

    this.http.post(this.domain + 'add', body)
      .subscribe( res => this.addCartResponse(res, item));
  }

  public emptyCart(): void {
    // TODO
  }

  public getItems() {
    let cart: string;
    this.cart.subscribe(id => cart = id);
    const headers = new HttpHeaders({
      'cart_id': cart
    });
    return this.http.get(this.domain + 'content', {headers: headers}).subscribe(res => res);
  }

  public async initializeCart() {
    this.http.get(this.domain + 'init', {responseType: 'text'})
    .subscribe( response => {
      this.setCart(response);
    });
  }

  public removeFromCart(remove): void {
    // TODO
  }



  // PRIVATE METHODS
  private addCartResponse(response, item: CrashReport) {
    const result = response.message;
    result === 'success' ?
      this.updateCart(item) :
      // this should become an unreachable code path
      console.log(result);
  }
  // whenever items are added or removed see if the cart should be accessible
  private cartStillActive() {
    let count, current;
    this.isActive.subscribe( a => current = a);
    this._items.subscribe( c => count = c.length);
    // cart contains items
    if (count > 0) {
      // cart is currently inactive
      if (!current) {
      this.Active.next(true);
    }}
    // cart is empty
    if (count === 0) {
      // cart is set as active
      if (current) {
      this.Active.next(false);
    }}
  }
  private setCart(cart) {
    this.Cart.next(cart);
  }

  private updateCart(item: CrashReport) {
    let items;
    this._items.subscribe(i => items = i);
    items.push(item);
    this.Items.next(items);
    this.cartStillActive();
  }
}
