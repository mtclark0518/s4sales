import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../providers/admin.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'request-container',
  templateUrl: './request-container.component.html',
  styleUrls: ['../admin.scss']
})
export class RequestContainerComponent implements OnInit {
  request_queue: Array<any>;
  constructor(private admin: AdminService) { }

  ngOnInit() {
    this.admin.requestQueue.subscribe(q => this.request_queue = q);
    this.admin.getAwaitingApproval();
  }

}
