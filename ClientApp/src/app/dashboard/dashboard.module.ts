import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SidePanelComponent } from './side-panel/side-panel.component';
import { TopPanelComponent } from './top-panel/top-panel.component';
import { FilterComponent } from './filter/filter.component';
import { OverviewComponent } from './overview/overview.component';
import { ChartingComponent } from './charting/charting.component';
import { ChartSelectComponent } from './chart-select/chart-select.component';
import { DisplayHeadComponent } from './display-head/display-head.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { ChartDirective } from '../directives/chart.directive';
@NgModule({
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, HighchartsChartModule
  ],
  declarations: [
    DashboardComponent, ChartDirective,
    SidePanelComponent, TopPanelComponent,
    FilterComponent, OverviewComponent, ChartingComponent,
    ChartSelectComponent, DisplayHeadComponent
  ],
  exports: [DashboardComponent]
})
export class DashboardModule { }
