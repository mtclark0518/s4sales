import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from '../../providers/search.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'search-header',
  templateUrl: './search-header.component.html',
  styleUrls: ['../search.scss']
})
export class SearchHeaderComponent implements OnInit {
  @Input('status') status: string;

  public SearchResults;

  constructor( private search: SearchService ) { }

  ngOnInit() {
    this.search.searchResults.subscribe(data => this.SearchResults = data);
  }
  public startOver() {
    this.search.updateSearchStatus('');
  }
}
