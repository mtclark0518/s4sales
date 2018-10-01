import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AdminService {

  headers = new HttpHeaders({
      'Accept': 'application/json',
      'Content-Type': 'application/json',
  });

  domain = 'http://localhost:5000/api/admin';

  private InReview = new BehaviorSubject<Object>({});
  private RequestQueue = new BehaviorSubject<Array<any>>([]);
  private Viewable = new BehaviorSubject<boolean>(false);

  public inReview = this.InReview.asObservable();
  public requestQueue = this.RequestQueue.asObservable();
  public isFormViewable = this.Viewable.asObservable();

  constructor( private http: HttpClient ) { }

  getAwaitingApproval() {
    this.http.get(this.domain + '/approval').subscribe(response => {
      this.setRequestQueue(response);
    });
  }

  getAwaitingVerification() {
    this.http.get(this.domain + '/verification').subscribe(response => {
      this.setRequestQueue(response);
    });
  }

  selectForReview(req) {
    this.InReview.next(req);
    this.Viewable.next(true);
  }

  setRequestQueue(req) {
    this.RequestQueue.next(req);
  }

  submitApprovalResponse(req) {
    this.http.post(this.domain + '/approve', req).subscribe(response => {
      // TODO
    });
  }

}

