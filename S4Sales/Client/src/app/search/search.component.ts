import { Component, OnInit } from '@angular/core';
import { SearchService } from '../providers/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.scss']
})
export class SearchComponent implements OnInit {
  public status: string;
  public SearchResults;


  constructor(private search: SearchService) { }

  ngOnInit() {
    this.search.Status.subscribe(status => this.status = status);
    this.search.searchResults.subscribe(data => this.SearchResults = data);

  }

  selectQueryType($event) {
    this.search.selectQueryType($event);
    this.search.updateSearchStatus('searching');
  }
  public startOver() {
    this.search.updateSearchStatus('');
  }
}



