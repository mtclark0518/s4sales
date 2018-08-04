import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { CrashEventComponent } from './crash-event/crash-event.component';
import { ContactComponent } from './contact/contact.component';
import { FaqsComponent } from './faqs/faqs.component';
import { TermsAndConditionsComponent } from './terms-and-conditions/terms-and-conditions.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app.routing';
import { CollapseModule, BsDropdownMenuDirective, BsDropdownModule } from 'ngx-bootstrap';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';

@NgModule({
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule,
    AppRoutingModule, CollapseModule, BsDropdownModule
  ],
  declarations: [
    NavbarComponent, CheckoutComponent,
    FooterComponent, CartComponent,
    CrashEventComponent, ContactComponent,
    FaqsComponent, TermsAndConditionsComponent,
  ],
  exports: [CartComponent , NavbarComponent, FooterComponent, CrashEventComponent]
})
export class ComponentsModule { }
