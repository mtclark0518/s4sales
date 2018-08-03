import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'top-panel',
  templateUrl: './top-panel.component.html',
  styleUrls: ['../dashboard.scss']
})
export class TopPanelComponent implements OnInit {

  constructor(private account: AccountService) { }

  ngOnInit() {
  }
  getCurrentUser() {
    console.log('get current user');
    this.account.getCurrentUser();
  }
}
