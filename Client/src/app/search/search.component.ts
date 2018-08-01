import { Component, OnInit } from '@angular/core';
import { SearchService } from '../providers/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.scss']
})
export class SearchComponent implements OnInit {
  public status: string;


  constructor(private search: SearchService) { }

  ngOnInit() {
    this.search.Status.subscribe(status => this.status = status);
  }

  selectQueryType($event) {
    this.search.selectQueryType($event);
    this.search.updateSearchStatus('searching');
  }
}
