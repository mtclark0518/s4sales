import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchComponent } from './search.component';
import { SearchHeaderComponent } from './search-header/search-header.component';
import { SearchFormComponent } from './search-form/search-form.component';
import { SearchResultsComponent } from './search-results/search-results.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule, TooltipModule } from 'ngx-bootstrap';
import { ComponentsModule } from '../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ComponentsModule,
    BsDatepickerModule, TooltipModule
  ],
  declarations: [SearchComponent, SearchHeaderComponent, SearchFormComponent, SearchResultsComponent]
})
export class SearchModule { }
