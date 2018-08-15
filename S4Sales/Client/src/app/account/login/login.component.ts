import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { CustomValidators } from '../../models/validators';
import { Credentials } from '../../models/_class';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../account.scss']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  private validator: CustomValidators;

  constructor(private formBuilder: FormBuilder, private account: AccountService) {
    this.validator = new CustomValidators();
   }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      'username': new FormControl(null, [Validators.required]),
      'password': new FormControl(null, [
        Validators.required,
        // this.validator.pwValidator(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/)
      ]),
    });
  }
  get username() { return this.loginForm.get('username'); }
  get password() { return this.loginForm.get('password'); }

  login($event): void {
    $event.preventDefault();
    const creds: Credentials = {
      user_name: this.username.value,
      password: this.password.value
    };
    this.account.login(creds);
  }

  recoverAccount(): void {
    this.account.changeStatus('lostPassword');
  }
}
