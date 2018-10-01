// tslint:disable:radix

import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../providers/admin.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Status } from '../../models/_enums';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'profile-builder',
  templateUrl: './profile-builder.component.html',
  styleUrls: ['../admin.scss']
})
export class ProfileBuilderComponent implements OnInit {
  active;
  requested;
  adminForm: FormGroup;
  constructor(private admin: AdminService, private fb: FormBuilder) { }

  ngOnInit() {
    this.admin.isFormViewable.subscribe(bool => this.active = bool);
    this.admin.inReview.subscribe(req => this.requested = req);
    this.adminForm = this.fb.group({
      'message': new FormControl(null, []),
      'status': new FormControl(null, []),
    });
  }

  get message() { return this.adminForm.get('message'); }
  get status() { return this.adminForm.get('status'); }

  submit(e: Event) {
    e.preventDefault();

    const request = {
      request_number: this.requested.request_number,
      request_type: this.requested.request_type,
      response_status: Status[parseInt(this.status.value)],
      message: this.message.value || 'approved',
    };

    this.admin.submitApprovalResponse(request);
  }

}
