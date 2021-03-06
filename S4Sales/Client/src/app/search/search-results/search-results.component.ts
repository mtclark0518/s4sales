import { Component, OnInit } from '@angular/core';
import { SearchService } from '../../providers/search.service';
import { CartService } from '../../providers/cart.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['../search.scss']
})
export class SearchResultsComponent implements OnInit {
  public Results;
  constructor( private search: SearchService, private cart: CartService) { }

  ngOnInit() {
    this.search.searchResults.subscribe( results => this.Results = results );
  }

  addToCart(item) {
    this.cart.addToCart(item);
  }
}
