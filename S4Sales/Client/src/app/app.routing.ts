import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './search/search.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { PasswordComponent } from './account/password/password.component';
import { AuthGuard } from './providers/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin/admin.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { HelpContainer } from './components/help/help-container.component';
import { ProfileComponent } from './account/profile/profile.component';

export const routes: Routes = [


  { path: '', component: SearchComponent },
  { path: 'help', component: HelpContainer },

  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'recover', component: PasswordComponent },

  { path: 'account',
    canActivate: [AuthGuard],
    children: [{ path: '', component: ProfileComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'reset', component: PasswordComponent }]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }
