import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { EcommerceService } from '../../providers/ecommerce.service';
import { SearchService } from '../../providers/search.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { CartService } from '../../providers/cart.service';
import { Transaction } from '../../models/_classes';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.scss']
})
export class CheckoutComponent implements OnInit, AfterViewInit {
  @ViewChild('card') card: ElementRef;
  private _card: any;
  public checkoutState;
  public Cart;
  public cart_id;
  public price = 10.25;
  public purchaseForm: FormGroup;

  constructor(
    private cart: CartService,
    private ecomm: EcommerceService,
    private search: SearchService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.cart._items.subscribe( i => this.Cart = i);
    this.cart.cart.subscribe(c=> this.cart_id = c);
    this.purchaseForm = this.fb.group({
      'first': new FormControl(null, [Validators.required]),
      'last': new FormControl(null, [Validators.required])
    });
  }

  ngAfterViewInit() {
    // creates stripe.js elements
    const elements = this.ecomm.stripe.elements();
    this._card = elements.create('card');
    // attaches stripes creditcard element
    this._card.mount(this.card.nativeElement);
  }

  get first() { return this.purchaseForm.get('first'); }
  get last() { return this.purchaseForm.get('last'); }

  total() { return (this.Cart.length * 1425); }

  clearCart = () => this.cart.emptyCart();

  continueShopping = () =>  this.search.updateSearchStatus('');

  async onSubmit($event) {
    $event.preventDefault();
    const created = await this.ecomm.createToken(this._card);
    if (created.token) {
      const order: Transaction = {
        first_name: this.first.value,
        last_name: this.last.value,
        amount: this.total(),
        token: created.token.id,
        cart_id: this.cart_id
      };
      this.ecomm.submit(order);
    } else {
      alert('nope');
    }
  }
}
