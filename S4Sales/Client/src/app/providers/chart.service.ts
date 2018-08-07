import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Overview } from './dashboard.service';


@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = {series: []};
  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();

  constructor( private http: HttpClient) {}

  setChartOptions(data: Overview) {
    console.log(data)

    const options = {
      chart: {
        type: 'column',
      },
      colors: ['#cf7ec8', '#1ea303'],

      title: {
        text: data.name
      },
      xAxis: {
        categories: ['Revenue']
      },
      yAxis: {
        min: 0,
        title: {
          text: 'Total Revenue'
        }
      },
      tooltip: {
        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
        shared: true

      },
      plotOptions: {
        column: {
          stacking: 'normal'
        }
      },
      series:
      [
        {
          name: 'HSMV',
          data: [data.total_revenue - data.total_reimbursed]
        }, {
          name: 'Reimbursed',
          data: [data.total_reimbursed]
        }
      ],
      // responsive: {}
    };
    this.ChartOptions.next(options);
  }
}
