import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { CrashReportComponent } from './crash-report/crash-report.component';
import { ContactComponent } from './help/contact.component';
import { FaqsComponent } from './help/faqs.component';
import { TermsAndConditionsComponent } from './help/terms-and-conditions.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app.routing';
import { CollapseModule, BsDropdownModule } from 'ngx-bootstrap';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { HelpContainerComponent } from './help/help-container.component';

@NgModule({
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule,
    AppRoutingModule, CollapseModule, BsDropdownModule
  ],
  declarations: [
    NavbarComponent, CheckoutComponent,
    FooterComponent, CartComponent,
    CrashReportComponent, ContactComponent,
    FaqsComponent, TermsAndConditionsComponent,
    HelpContainerComponent
  ],
  exports: [
    CartComponent, NavbarComponent,
    FooterComponent, CrashReportComponent,
  ]
})
export class ComponentsModule { }
