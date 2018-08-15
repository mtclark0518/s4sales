import { Component, OnInit, Input } from '@angular/core';
import { AdminService } from '../../providers/admin.service';
import { AccountRequestType } from '../../models/_enum';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'new-account-request',
  templateUrl: './new-account-request.component.html',
  styleUrls: ['../admin.scss']
})
export class NewAccountRequestComponent implements OnInit {
  @Input('details') details;
  requestType = AccountRequestType;
  constructor(private admin: AdminService) { }

  ngOnInit() {}

  selectForReview() {
    this.admin.selectForReview(this.details);
  }
}
