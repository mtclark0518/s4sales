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
  public Highcharts = Highcharts;
  public Options;
  public updateFlag;
  constructor( private chart: ChartService ) {}

  ngOnInit() {
    this.chart.chartOptions.subscribe( chartOpts => this.Options = chartOpts );
    this.chart.shouldUPDATE.subscribe( bool => this.updateFlag = bool );
  }

}

