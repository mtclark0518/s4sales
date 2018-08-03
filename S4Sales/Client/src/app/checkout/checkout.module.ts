import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartComponent } from './cart/cart.component';
import { AppRoutingModule } from '../app.routing';
import { CheckoutComponent } from './checkout.component';
import { ReactiveFormsModule, FormsModule } from '../../../node_modules/@angular/forms';

@NgModule({
  imports: [
    CommonModule, AppRoutingModule, FormsModule, ReactiveFormsModule
  ],

  declarations: [CartComponent, CheckoutComponent],

  exports: [CartComponent]
})
export class CheckoutModule { }
