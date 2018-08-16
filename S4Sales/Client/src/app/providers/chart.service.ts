import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { formatDate } from '@angular/common';
import { Month, ChartType } from '../models/_enum';
import { Overview } from '../models/_class';
      // tslint:disable:radix




@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = {
        series: [{
            name: 'HSMV',
            data: [],
          }, {
            name: 'Local',
            data: [],
          }
        ]
  };

  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();
  private UPDATE = new BehaviorSubject<boolean>(false);
  public shouldUPDATE = this.UPDATE.asObservable();
  constructor( private http: HttpClient) {}


  setChartOptions(data: Overview) {


    if (data['details']) {
      console.log(data['details']);
      const xaxis = [];
      const _keys = Object.keys(data.month_count);
      _keys.forEach( key => xaxis.push(Month[parseInt(key) - 1]));

      const options: Highcharts.Options = {
        chart: {
          type: data.chart
        },
        title: {
          text: data.name
        },
        xAxis: {
          categories: xaxis
        },
        colors: ['#269AAE', '#8a8a91'],

        plotOptions: {
          column: {
            stacking: 'normal'
          }
        },
      };



      switch (data.details['chart_name']) {
        case ChartType.Reimbursements :
          console.log('thisis is is is isiiiiiis reimbbbursing');


          options['series'] = [
            {
              name: 'HSMV',
              data: [data.total_revenue - data.total_reimbursed]
            }, {
              name: 'Local',
              data: [data.total_reimbursed]
            }
          ];
          options['yAxis'] = {
            min: 0,
            title: {
              text: 'Total Reimbursements'
            }
          };

        break;

        case ChartType.Reports :
          console.log('reeeportsings');


          const _values = Object.values(data.month_count);
          options['series'] = [{name: 'Reports', data: _values}, {name: 'Timely', data: [0]}];
          options['yAxis'] = {
            min: 0,
            title: {
              text: 'Reports Entered'
            }
          };

        break;
        default : console.log('this means that its not working yet');
      }
      //   colors: ['#ff7e08', '#1aad00'],
      //   tooltip: {
      //     pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
      //     shared: true
      //   },
      //   plotOptions: {
      //     column: {
      //       stacking: 'normal'
      //     }
      //   },
      this.ChartOptions.next(options);
    }
  }

  // what options are consistent across renders




}

