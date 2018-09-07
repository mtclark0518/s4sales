import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { AgencyAccount } from '../../models/_classes';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isExpanded = false;
  account: AgencyAccount;

  constructor( private acc: AccountService ) {}
  ngOnInit() {
    this.acc.currentAccount.subscribe(account => this.account = account);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
