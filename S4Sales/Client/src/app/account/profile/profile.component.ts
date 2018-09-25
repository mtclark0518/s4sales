import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { AgencyAccount, OnboardingDetails } from '../../models/_classes';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['../account.scss']
})
export class ProfileComponent implements OnInit, AfterViewChecked {
  profile: AgencyAccount;
  details: OnboardingDetails;
  shouldOnboard = true;
  constructor( private account: AccountService) { }

  ngOnInit() {
    this.account.details.subscribe(details => this.profile = details);
    this.account.onboardingDetails.subscribe(details => this.details = details);
  }
  ngAfterViewChecked() {
    if (this.details !== null) {
      if (this.shouldOnboard) {
        this.account.onboard(this.details);
        this.shouldOnboard = false;
      }
    }
  }

  sendToOnboarding() {
    const link = window.document.createElement('a');
    link.href = 'https://connect.stripe.com/oauth/authorize?response_type' +
      '=code&client_id=ca_DK4LfgjwY5CxXBlZetcDNriX0eW0Zs2M&scope=read_write&state=' +
      this.profile.agency_id.toString();
      link.click();
  }

}
