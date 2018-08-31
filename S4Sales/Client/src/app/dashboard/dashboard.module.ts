import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SidePanelComponent } from './side-panel/side-panel.component';
import { TopPanelComponent } from './top-panel/top-panel.component';
import { FilterComponent } from './filter/filter.component';
import { ChartingComponent } from './charting/charting.component';
import { ChartSelectComponent } from './chart-select/chart-select.component';
import { DisplayHeadComponent } from './display-head/display-head.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { ComponentsModule } from '../components/components.module';
import { ReportingComponent } from './reporting/reporting.component';
import { BsDatepickerModule } from 'ngx-bootstrap';
@NgModule({
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, HighchartsChartModule, ComponentsModule, BsDatepickerModule
  ],
  declarations: [
    DashboardComponent,
    SidePanelComponent, TopPanelComponent,
    FilterComponent, ChartingComponent,
    ChartSelectComponent, DisplayHeadComponent, ReportingComponent
  ],
  exports: [DashboardComponent]
})
export class DashboardModule { }
