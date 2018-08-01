import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing';
import { SearchModule } from './search/search.module';
import { ComponentsModule } from './components/components.module';
import { HttpClientModule } from '@angular/common/http';
import { CollapseModule, BsDropdownModule, AlertModule, TooltipModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AccountModule } from './account/account.module';
import { AdminModule } from './admin/admin.module';
@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule, AppRoutingModule, HttpClientModule,
    ComponentsModule, SearchModule, AccountModule, AdminModule,
    BsDatepickerModule.forRoot(), CollapseModule.forRoot(),
    AlertModule.forRoot(), TooltipModule.forRoot(),
    BsDropdownModule.forRoot()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
