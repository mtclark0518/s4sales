import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CollapseModule, BsDropdownModule } from 'ngx-bootstrap';
import { AppRoutingModule } from '../app.routing';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { ContactComponent } from './help/contact.component';
import { CrashReportComponent } from './crash-report/crash-report.component';
import { DownloadComponent } from './download/download.component';
import { FooterComponent } from './footer/footer.component';
import { FaqsComponent } from './help/faqs.component';
import { HelpContainer } from './help/help-container.component';
import { NavbarComponent } from './navbar/navbar.component';
import { TermsAndConditionsComponent } from './help/terms-and-conditions.component';

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
    HelpContainer, DownloadComponent
  ],
  exports: [
    CartComponent, NavbarComponent,
    FooterComponent, CrashReportComponent
  ]
})
export class ComponentsModule { }
