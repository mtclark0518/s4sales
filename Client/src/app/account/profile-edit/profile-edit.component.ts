import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['../account.scss']
})
export class ProfileEditComponent implements OnInit {

  public editProfile: FormGroup;

  public formData = {username: null};

  constructor() { }

  ngOnInit() {
    this.editProfile = new FormGroup({
      'username': new FormControl(this.formData.username)
    });
  }

  get name() { return this.editProfile.get('username'); }

}
