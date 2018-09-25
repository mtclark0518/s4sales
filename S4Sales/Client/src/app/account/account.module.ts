import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PasswordComponent } from './password/password.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app.routing';
import { TooltipModule, TypeaheadModule } from 'ngx-bootstrap';
import { DashboardModule } from '../dashboard/dashboard.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule, ReactiveFormsModule,
    AppRoutingModule, TooltipModule,
    TypeaheadModule, DashboardModule,
  ],
  declarations: [ PasswordComponent, LoginComponent, RegisterComponent]
})
export class AccountModule { }
