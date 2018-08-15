import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { formatDate } from '@angular/common';
import { Month } from '../models/_enum';
import { Overview } from '../models/_class';
      // tslint:disable:radix




@Injectable({
  providedIn: 'root'
})

export class ChartService {

  private initial: Highcharts.Options = { series: [{ data: [1, 2]}] };

  private ChartOptions = new BehaviorSubject<Highcharts.Options>(this.initial);
  public chartOptions = this.ChartOptions.asObservable();

  constructor( private http: HttpClient) {}


  setChartOptions(data: Overview) {
    const keys = Object.keys(data.month_count);
    const xaxis = [];
    const series = [];
    let month;

    keys.forEach( key => {
      const mn = parseInt(key);
      month = Month[mn - 1];
      xaxis.push(month);
    });

    const values = Object.values(data.month_count);
    const dt = [];
    const cht = {};

    values.forEach( val => dt.push(val) );
    cht['data'] = dt;
    series.push(cht);


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
      series: series
    };
    console.log(options);
    this.ChartOptions.next(options);
  }

  // what type of chart should i render

  // what options are consistent across renders




}

