import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './search/search.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { PasswordComponent } from './account/password/password.component';
import { AuthGuard, AdminGuard } from './providers/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin/admin.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { HelpContainerComponent } from './components/help-container/help-container.component';
import { ProfileComponent } from './account/profile/profile.component';

export const routes: Routes = [


  { path: '', component: SearchComponent },
  { path: 'help', component: HelpContainerComponent },

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
