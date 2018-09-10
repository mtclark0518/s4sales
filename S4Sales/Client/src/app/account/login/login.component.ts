import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { CustomValidators } from '../../models/validators';
import { Credentials } from '../../models/_classes';
import { FDOT_AGENCIES } from '../../models/fdot.enum';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../account.scss']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  private validator: CustomValidators;
  public agencies = [];

  constructor(private formBuilder: FormBuilder, private account: AccountService) {
    this.validator = new CustomValidators();
   }

  ngOnInit() {
    this.agencies = Object.values(FDOT_AGENCIES);

    this.loginForm = this.formBuilder.group({
      'agency': new FormControl(null, [Validators.required]),
      'password': new FormControl(null, [
        Validators.required,
        // this.validator.pwValidator(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/)
      ]),
    });
  }
  get agency() { return this.loginForm.get('agency'); }
  get password() { return this.loginForm.get('password'); }

  login($event): void {
    $event.preventDefault();
    const creds: Credentials = {
      agency_name: this.agency.value,
      password: this.password.value
    };
    this.account.login(creds);
  }

  recoverAccount(): void {
    this.account.changeStatus('lostPassword');
  }
}
