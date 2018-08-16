import {Component, OnInit} from '@angular/core';
import { ChartService } from '../../providers/chart.service';
import * as Highcharts from 'highcharts';
import { DashboardService } from '../../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'charting',
  templateUrl: './charting.component.html',
  styleUrls: ['../dashboard.scss']
})

export class ChartingComponent implements OnInit {
  Highcharts = Highcharts;
  Options: Highcharts.Options;

  constructor( private chart: ChartService, private dash: DashboardService ) {}
  ngOnInit() {
    this.chart.chartOptions.subscribe( chartOpts => this.Options = chartOpts );
  }

}

