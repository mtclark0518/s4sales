import { Component, OnInit } from '@angular/core';
import { AlertService } from './alert.service';

@Component({
  selector: 'alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements OnInit {
  public message;
  public displayed;
  constructor(private alert: AlertService) { }

  ngOnInit() {
    this.alert.message.subscribe(content => this.message = content);
    this.alert.isDisplayed.subscribe(status => this.displayed = status);
    this.alert.setAlert('test');
  }

  public dismiss() {
    this.alert.display(false);
  }

}
