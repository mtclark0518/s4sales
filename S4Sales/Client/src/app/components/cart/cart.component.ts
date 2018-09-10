import { Component, OnInit } from '@angular/core';
import { CartService } from '../../providers/cart.service';
import { CrashReport } from '../../models/_interfaces';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  isActive: boolean;
  items: Array<CrashReport>;

  constructor( private cart: CartService) { }

  ngOnInit() {
    this.cart.initializeCart();
    this.cart.isActive.subscribe( state => this.isActive = state);
    this.cart._items.subscribe(i => this.items = i);
  }



}
