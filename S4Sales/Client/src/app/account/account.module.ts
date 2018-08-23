import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PasswordComponent } from './password/password.component';
import { ProfileComponent } from './profile/profile.component';
import { ProfileEditComponent } from './profile-edit/profile-edit.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app.routing';
import { TooltipModule } from 'ngx-bootstrap';
import { DashboardModule } from '../dashboard/dashboard.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule, ReactiveFormsModule,
    AppRoutingModule, TooltipModule, DashboardModule,
  ],
  declarations: [ PasswordComponent, ProfileComponent, ProfileEditComponent, LoginComponent, RegisterComponent]
})
export class AccountModule { }
