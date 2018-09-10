import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['../account.scss']
})
export class ProfileComponent implements OnInit {
  account;
  constructor( private acc: AccountService) { }

  ngOnInit() {
    this.acc.currentAccount.subscribe(acc => this.account = acc);
  }

}
