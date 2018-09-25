import { NgModule } from '@angular/core';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { ChartingComponent } from './charting/charting.component';
import { ChartSelectComponent } from './chart-select/chart-select.component';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../components/components.module';
import { DashboardComponent } from './dashboard.component';
import { FilterComponent } from './filter/filter.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HighchartsChartModule } from 'highcharts-angular';
import { ProfileComponent } from './profile/profile.component';
import { ReportingComponent } from './reporting/reporting.component';
import { SidePanelComponent } from './side-panel/side-panel.component';
import { SwitchTabsComponent } from './switch-tabs/switch-tabs';
import { TopPanelComponent } from './top-panel/top-panel.component';


@NgModule({
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, HighchartsChartModule, ComponentsModule, BsDatepickerModule
  ],
  declarations: [
    DashboardComponent, ProfileComponent,
    SidePanelComponent, TopPanelComponent,
    FilterComponent, ChartingComponent,
    ChartSelectComponent, SwitchTabsComponent, ReportingComponent
  ],
  exports: [DashboardComponent]
})
export class DashboardModule { }
