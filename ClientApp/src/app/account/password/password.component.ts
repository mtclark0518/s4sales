import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Password } from '../../models/password';
import { AccountService } from '../../providers/account.service';

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
      'accountEmail': new FormControl(this.recoverAccount),
      'oldPassword': new FormControl(this.oldPassword),
      'newPassword': new FormControl(this.newPassword),
      'confirmPassword': new FormControl(this.confirmPassword),
    });
  }

  get accountEmail() {  return this.passwordForm.get('accountEmail'); }
  get oldPass() { return this.passwordForm.get('oldPassword'); }
  get newPass() { return this.passwordForm.get('newPassword'); }
  get confirmPass() { return this.passwordForm.get('confirmPassword'); }

  // confirms the new password and submits the change. service redirects to profile
  submitPassword($event): void {
    if (this.newPass.value !== this.confirmPass.value) {
      alert('new passwords do not match. try again');
    }
    $event.preventDefault();
    switch (this.status) {
      case 'lostPassword':
      const recover = {
        account: this.accountEmail.value
      };
      this.account.recoverAccount(recover);
      break;
      case 'changePassword':
      const change: Password = {
        oldPassword: this.oldPass.value,
        newPassword: this.newPass.value,
      };
      this.account.changePassword(change);
        break;
      case 'resetPassword':
        const reset: Password = {
          newPassword: this.newPass.value,
        };
        this.account.changePassword(reset);
        break;
        default:
        console.log('the default is an error');
    }
  }

}
