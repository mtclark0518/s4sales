import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { EcommerceService } from '../../providers/ecommerce.service';
import { SearchService } from '../../providers/search.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Transaction } from '../../models/transaction';
import { CartService } from '../../providers/cart.service';

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
  public PurchaseForm: FormGroup;

  constructor(
    private cart: CartService,
    private ecomm: EcommerceService,
    private search: SearchService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.cart._items.subscribe( i => this.Cart = i);
    this.cart.cart.subscribe(c=> this.cart_id = c);
    console.log(this.Cart);
    this.PurchaseForm = this.fb.group({
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

  get first() { return this.PurchaseForm.get('first'); }
  get last() { return this.PurchaseForm.get('last'); }

  total() { return (this.Cart.length * 16) * 100; }

  clearCart = () => this.cart.emptyCart();

  continueShopping = () =>  this.search.updateSearchStatus('');

  async onSubmit($event) {
    $event.preventDefault();
    const created = await this.ecomm.createToken(this._card);
    if (created.token) {
      console.log(created.token.id);
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
