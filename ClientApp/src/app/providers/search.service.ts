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



  private domain = 'http://localhost:5000/api';

  private QueryType = new BehaviorSubject<string>('');
  public queryType = this.QueryType.asObservable();

  private SearchStatus = new BehaviorSubject<string>('');
  public Status = this.SearchStatus.asObservable();

  private SearchResults = new BehaviorSubject<Array<CrashEvent>>([]);
  public searchResults = this.SearchResults.asObservable();

  public updateSearchStatus($event) {
    this.SearchStatus.next($event);
  }

  public selectQueryType($event) {
    this.QueryType.next($event);
  }

  public submitSearch(query: SearchQuery) {
    console.log(query);
    let headers: HttpHeaders;
    if (query.queryType === 'report') {
      headers = new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'hsmv': query.reportNumber
      });
      this.http.get(this.domain + '/crash/' + query.queryType, {headers: headers}).subscribe(response => {
        this.handleOne(response);
      });
    }
    if (query.queryType === 'vehicle') {
      headers = new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'vin': query.vehicleVIN
      });
      console.log(headers)
      this.http.get(this.domain + '/crash/' + query.queryType, {headers: headers}).subscribe(response => {
        this.handleMany(response);
      });
    }
    if (query.queryType === 'detailed') {
      console.log(typeof(query.crashDate))
      headers = new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'crash' : query.crashDate.toString(),
        'participant' : query.participant.trim().toUpperCase()
      });

      console.log(headers)
      this.http.get(this.domain + '/crash/' + query.queryType, {headers: headers}).subscribe(response => {
        this.handleMany(response);
      });
    }
  }

  private handleOne(data) {
    console.log(data);
    const arr = [];
    arr.push(data);
    this.SearchResults.next(arr);
    this.SearchStatus.next('returning');
  }
  private handleMany(data) {
    console.log(data);
    const arr = [];
    data.forEach(item => {
      arr.push(item);
    });
    this.SearchResults.next(arr);
    this.SearchStatus.next('returning');
  }

}
