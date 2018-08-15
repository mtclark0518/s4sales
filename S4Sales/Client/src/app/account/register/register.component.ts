import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../providers/account.service';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from '../../models/validators';
import { AccountRequestType } from '../../models/_enum';
import { S4Request } from '../../models/_class';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../account.scss']
})
export class RegisterComponent implements OnInit {
  public registrationForm: FormGroup;
  public validator: CustomValidators;
  public requestTypes = AccountRequestType;
  constructor(private formBuilder: FormBuilder, private account: AccountService) {
    this.validator = new CustomValidators();
  }

  ngOnInit() {
    console.log(this.requestTypes);
    this.registrationForm = this.formBuilder.group({
      'user_name': new FormControl(null, [Validators.required]),
      'email': new FormControl(null, [Validators.required]),
      'first_name': new FormControl(null, [Validators.required]),
      'last_name': new FormControl(null, [Validators.required]),
      'organization': new FormControl(null),
      'request_type': new FormControl(null)
    });
  }
  get email() { return this.registrationForm.get('email'); }
  get first_name() { return this.registrationForm.get('first_name'); }
  get last_name() { return this.registrationForm.get('last_name'); }
  get organization() { return this.registrationForm.get('organization'); }
  get request_type() { return this.registrationForm.get('request_type'); }
  get user_name() { return this.registrationForm.get('user_name'); }


  updateView() {
    console.log(this.request_type.value);
    console.log(AccountRequestType[0]);
  }

  register($event) {
    $event.preventDefault();
      const account: S4Request = {
        email: this.email.value,
        first_name: this.first_name.value,
        last_name: this.last_name.value,
        organization: this.organization.value,
        request_type: this.request_type.value,
        user_name: this.user_name.value
      };
      console.log(account);
      this.account.register(account);
    }
}
