import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from '../../models/validators';
import { AccountRequestType } from '../../models/_enums';
import { NewAgency } from '../../models/_classes';
import { FDOT_AGENCIES } from '../../models/fdot.enum';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../account.scss']
})
export class RegisterComponent implements OnInit {
  public registrationForm: FormGroup;
  public validator: CustomValidators;
  public requestTypes = AccountRequestType;
  public agencies = [];
  constructor(
    private formBuilder: FormBuilder,
    private account: AccountService) {
      this.validator = new CustomValidators();
  }

  ngOnInit() {
    this.agencies = Object.values(FDOT_AGENCIES);

    this.registrationForm = this.formBuilder.group({
      'agency_name': new FormControl(null, [Validators.required]),
      'email': new FormControl(null, [Validators.required]),
      'first_name': new FormControl(null, [Validators.required]),
      'last_name': new FormControl(null, [Validators.required]),
      'password': new FormControl(null, [Validators.required]),
    });
  }
  get email() { return this.registrationForm.get('email'); }
  get first_name() { return this.registrationForm.get('first_name'); }
  get last_name() { return this.registrationForm.get('last_name'); }
  get agency_name() { return this.registrationForm.get('agency_name'); }
  get password() { return this.registrationForm.get('password'); }


  register($event) {
    $event.preventDefault();
      const account: NewAgency = {
        agency_name: this.agency_name.value,
        first_name: this.first_name.value,
        last_name: this.last_name.value,
        email: this.email.value,
        password: this.password.value
      };
      this.account.register(account);
    }
}
