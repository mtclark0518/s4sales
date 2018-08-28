import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { Month, ChartType } from '../models/_enums';
import { Overview } from '../models/_classes';
// tslint:disable:radix
@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = {
    chart: {
      backgroundColor: 'rgba(255,255,255,1)',
      plotBackgroundColor: 'rgba(250,250,250,0.5)',
    },
    title: {},
    plotOptions: {column: {stacking: 'normal'}},
    xAxis: {},
    yAxis: { min: 0, title: {} },
    colors: ['#C2493C', '#8a8a91'],
    series: [{}, {}]
  };

  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();
  private UPDATE = new BehaviorSubject<boolean>(false);
  public shouldUPDATE = this.UPDATE.asObservable();

  constructor() {}


  setChartOptions(opts: Overview) {
    // initialize the chart options
    let options;
    this.chartOptions.subscribe(current => options = current);
    if (opts['details']) {
      const month_keys = [];
      Object.keys( opts['month_count']).forEach( key => month_keys.push( Month[parseInt(key) - 1] ) );
      const _values = Object.values(opts['month_count']);


      // set chart options
      switch (opts.details['chart_type']) {
        case ChartType.Reimbursements :
          options['chart']['type'] = 'column';
          options['title']['text'] = 'Monthly Sales';
          options['xAxis']['categories'] = month_keys;
          options['yAxis']['title']['text'] = 'Total Reimbursements';
          options['series'] = [
            { name: 'HSMV', data: [(opts['total_count'] * 16) - opts['total_reimbursed']] },
            { name: 'Local', data: [opts['total_reimbursed']] }
          ];
        break;

        case ChartType.Reporting :
          options['chart']['type'] = 'spline';
          options['title']['text'] = 'Monthly Reporting';
          options['xAxis']['categories'] = month_keys;
          options['yAxis']['title']['text'] = 'Reports Entered';
          options['series'] = [{name: 'Reports', data: _values}, { name: 'Timely', data: []}];
        break;
        default : console.log('this means that its not working yet');
      }

      this.ChartOptions.next(options);
      this.UPDATE.next(true);
    }
  }
}

