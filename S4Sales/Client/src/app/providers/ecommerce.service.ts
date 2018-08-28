import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../models/_classes';

// declare stripe variable bc stripe.js has to be added to index.html
declare const Stripe: any;

@Injectable({
  providedIn: 'root'
})
export class EcommerceService {
  private domain = 'http://localhost:5000/api/commerce/';
  public stripe = Stripe('pk_test_OFR63JYG6hjiehXdVkgprAin');
  constructor(private http: HttpClient) { }

  createToken(card) {
    return this.stripe.createToken(card).then(res => {
        return res;
    });
  }
  submit(order: Transaction) {
    this.http.post(this.domain + 'create', order)
      .subscribe(response => {
        console.log(response);
      });
  }
}
