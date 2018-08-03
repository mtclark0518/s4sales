import { Component, OnInit } from '@angular/core';
import { CartService } from '../../providers/cart.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'cart',
  templateUrl: './cart.component.html',
  styleUrls: ['../checkout.scss']
})
export class CartComponent implements OnInit {
  isActive: boolean;
  constructor( private cart: CartService) { }

  ngOnInit() {
    this.cart.isActive.subscribe( state => this.isActive = state);
  }



}
