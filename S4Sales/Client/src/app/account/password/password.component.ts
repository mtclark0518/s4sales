import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AccountService } from '../../providers/account.service';
import { Password } from '../../models/_interfaces';

@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['../account.scss']
})
export class PasswordComponent implements OnInit {

  public status: string;
  public recoverAccount: string;
  public oldPassword: string;
  public newPassword: string;
  public confirmPassword: string;
  public passwordForm: FormGroup;

  constructor( private account: AccountService) {}

  ngOnInit() {
    this.account.accountStatus.subscribe(status => this.status = status);
    this.passwordForm = new FormGroup({
      'accountEmail': new FormControl(null),
      'oldPassword': new FormControl(null),
      'newPassword': new FormControl(null),
      'confirmPassword': new FormControl(null),
    });
  }

  get accountEmail() {  return this.passwordForm.get('accountEmail'); }
  get oldPass() { return this.passwordForm.get('oldPassword'); }
  get newPass() { return this.passwordForm.get('newPassword'); }
  get confirmPass() { return this.passwordForm.get('confirmPassword'); }

  // confirms the new password and submits the change. service redirects to profile
  submitPassword($event): void {
    $event.preventDefault();
    if (this.newPass.value !== this.confirmPass.value) {
      alert('new passwords do not match. try again');
    }
    switch (this.status) {
      case 'recover':
        const recover = {
          account: this.accountEmail.value
        };
        this.account.recoverAccount(recover);
        break;
      case 'change':
        const change: Password = {
          oldPassword: this.oldPass.value,
          newPassword: this.newPass.value,
        };
        this.account.changePassword(change);
        break;
    }
  }

}
