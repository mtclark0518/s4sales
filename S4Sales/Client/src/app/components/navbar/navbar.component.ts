import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { AccountProfile } from '../../models/_class';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isExpanded = false;
  account: AccountProfile;

  constructor( private acc: AccountService ) {}
  ngOnInit() {
    // this.acc.restoreSession();
    this.acc.currentAccount.subscribe(account => this.account = account);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
