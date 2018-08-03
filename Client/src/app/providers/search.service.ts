import { Injectable } from '@angular/core';
// tslint:disable-next-line:import-blacklist
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { CrashEvent } from '../models/crash-event';
import { SearchQuery } from '../models/search-query';

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  constructor( private http: HttpClient, private router: Router ) {}

  private domain = 'http://localhost:5000/api/crash/';

  private QueryType = new BehaviorSubject<string>('');
  public queryType = this.QueryType.asObservable();

  private SearchStatus = new BehaviorSubject<string>('');
  public Status = this.SearchStatus.asObservable();

  private SearchResults = new BehaviorSubject<Array<CrashEvent>>([]);
  public searchResults = this.SearchResults.asObservable();
  getHeaders() {
    return new HttpHeaders({
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    });
  }
  public selectQueryType($event) {
    this.QueryType.next($event);
  }

  public submitSearch(query: SearchQuery) {
    const headers = this.getHeaders();
    if (query.queryType === 'report') {
      headers.append('hsmv', query.reportNumber);
      this.http.get(this.domain + query.queryType, {headers: headers})
      .subscribe( response => {
        this.handleOne(response);
      });
    }
    if (query.queryType === 'vehicle') {
      headers.append('vin', query.vehicleVIN);
      this.http.get(this.domain + query.queryType, {headers: headers})
      .subscribe( response => {
        this.handleMany(response);
      });
    }
    if (query.queryType === 'detailed') {
      headers.append('crash', query.crashDate.toString());
      headers.append('participant', query.participant.trim().toUpperCase());

      this.http.get(this.domain + query.queryType, {headers: headers})
      .subscribe( response => {
        this.handleMany(response);
      });
    }
  }

  public updateSearchStatus($event) {
    this.SearchStatus.next($event);
  }

  private handleOne(data) {
    const arr = [];
    arr.push(data);
    this.SearchResults.next(arr);
    this.SearchStatus.next('returning');
  }

  private handleMany(data) {
    const arr = [];
    data.forEach(item => {
      arr.push(item);
    });
    this.SearchResults.next(arr);
    this.SearchStatus.next('returning');
  }

}
