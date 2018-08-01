import {Component, OnInit} from '@angular/core';
import { ChartService } from '../../providers/chart.service';
import * as Highcharts from 'highcharts';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'charting',
  templateUrl: './charting.component.html',
  styleUrls: ['../dashboard.scss']
})

export class ChartingComponent implements OnInit {
  Highcharts = Highcharts;
  Options: Highcharts.Options;

  constructor( private chart: ChartService ) {}
  ngOnInit(): void {
    this.chart.chartOptions.subscribe( chartOpts => this.Options = chartOpts );
  }
}
