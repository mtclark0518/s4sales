import { Component, OnInit } from '@angular/core';
import { CartService } from '../../providers/cart.service';
import { Cart } from '../../models/cart';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'cart',
  templateUrl: './cart.component.html',
  styleUrls: ['../checkout.scss']
})
export class CartComponent implements OnInit {
  public myCart: Cart;

  constructor( private cart: CartService) { }

  ngOnInit() {

    this.cart.cart.subscribe(cart => this.myCart = cart);
    this.cart.restoreCart();
  }



}
