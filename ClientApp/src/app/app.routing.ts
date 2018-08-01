import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './search/search.component';
import { FaqsComponent } from './components/faqs/faqs.component';
import { TermsAndConditionsComponent } from './components/terms-and-conditions/terms-and-conditions.component';
import { ContactComponent } from './components/contact/contact.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { PasswordComponent } from './account/password/password.component';
import { ProfileComponent } from './account/profile/profile.component';
import { AuthGuard, AdminGuard } from './providers/auth.guard';
import { CheckoutComponent } from './checkout/checkout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin/admin.component';
import { RedirectComponent } from './account/mi-response/mi-response.component';

export const routes: Routes = [


  { path: '', component: SearchComponent },
  { path: 'faq', component: FaqsComponent },
  { path: 'terms-and-conditions', component: TermsAndConditionsComponent },
  { path: 'contact', component: ContactComponent },

  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'request', component: RedirectComponent },

  { path: 'account',
    canActivate: [AuthGuard],
    children: [{ path: '', component: ProfileComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'setup/:s4id', component: PasswordComponent }
    ]
  },

  { path: 'admin',
    canActivate: [AdminGuard],
    children: [
      {path: '', component: AdminComponent},
      {path: 'dashboard', component: DashboardComponent}
    ]
  },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  providers: [AuthGuard, AdminGuard]
})
export class AppRoutingModule { }
