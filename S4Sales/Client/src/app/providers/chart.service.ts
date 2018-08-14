import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Overview } from './dashboard.service';
import { formatDate } from '@angular/common';
      // tslint:disable:radix


export enum Month {
  January,
  February,
  March,
  April,
  May,
  June,
  July,
  August,
  September,
  October,
  November,
  December
}

@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = {series: []};
  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();

  constructor( private http: HttpClient) {}

  setChartOptions(data: Overview) {
    const xax = [];
    const series = [];
    let month;
    const k = Object.keys(data.count);

    k.forEach( j => {
      const p = parseInt(j);
      month = Month[p - 1];
      xax.push(month);
    });

    const l = Object.values(data.count);
    l.forEach((v, i) => {
      const m = {};
      const n = [];
      n.push(v);
      m[Month[i]] = n;
      series.push(m);
    });

    console.log(series);
    const options = {
      chart: {
        type: 'bar',
      },

      colors: ['#cf7ec8', '#1ea303'],

      title: {
        text: data.name
      },

      xAxis: {
        categories: xax
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
          data: [100]
        }, {
          name: 'Reimbursed',
          data: [200]
        }
      ],
    };
    console.log(options);
    this.ChartOptions.next(options);
  }
}

