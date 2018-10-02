import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../models/_classes';
import { BehaviorSubject } from 'rxjs';

// declare stripe variable bc stripe.js has to be added to index.html
declare const Stripe: any;

@Injectable({
  providedIn: 'root'
})
export class EcommerceService {
  private domain = 'http://localhost:5000/api/commerce/';
  public stripe = Stripe('pk_test_OFR63JYG6hjiehXdVkgprAin');

  private DownloadTokens = new BehaviorSubject<Array<string>>([]);
  public tokens = this.DownloadTokens.asObservable();

  constructor(private http: HttpClient) { }


  createToken(card) {
    return this.stripe.createToken(card).then(res => {
        return res;
    });
  }
  public submit = (order: Transaction): void => {
    this.http.post(this.domain + 'create', order)
      .subscribe(response => {
        // TODO
        this.handleOrder(response);
      });
  }

  private handleOrder = order => {
    console.log(order);
    if (order) {
      this.DownloadTokens.next(order);
    }
  }
}
