import { Component , OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  ip;
  constructor(private http: HttpClient) {

  }
  ngOnInit() {
    this.ipLocate();
  }

  ipLocate() {
    return this.http.get('http://ip-api.com/json')
      .subscribe(response => {
        this.setIP(response);
      });
  }
  setIP(ip) {
    this.ip = ip.query;
    console.log(this.ip);

  }
}
