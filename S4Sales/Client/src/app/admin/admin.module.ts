import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewAccountRequestComponent } from './new-account-request/new-account-request.component';
import { AdminComponent } from './admin.component';
import { RequestContainerComponent } from './request-container/request-container.component';
import { ProfileBuilderComponent } from './profile-builder/profile-builder.component';
import { ReactiveFormsModule } from '../../../node_modules/@angular/forms';

@NgModule({
  imports: [
    CommonModule, ReactiveFormsModule
  ],
  declarations: [NewAccountRequestComponent, AdminComponent, RequestContainerComponent, ProfileBuilderComponent],
  exports: [AdminComponent]
})
export class AdminModule { }
