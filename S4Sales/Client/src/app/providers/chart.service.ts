import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = {series: []};
  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();

  constructor( private http: HttpClient) {}

  setChartOptions(data) {
    const options = {
      chart: {
        type: 'column',
      },
      colors: ['#ff7e08', '#1aad00'],

      title: {
        text: 'Revenue'
      },
      xAxis: {
        categories: ['April', 'May', 'June', 'July']
      },
      yAxis: {
        min: 0,
        title: {
          text: 'Total Reimbursements'
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
          name: 'Local',
          data: [20, 21, 26, 33]
        }, {
          name: 'DHSMV',
          data: [12, 12, 13, 16]
        }
      ],
      // responsive: {}
    };
    this.ChartOptions.next(options);
  }
}
