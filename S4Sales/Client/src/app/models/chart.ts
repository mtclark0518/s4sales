import * as Highcharts from 'highcharts';
import { AsyncSubject } from 'rxjs';
import { ElementRef } from '@angular/core';

export type Point = number | [number, number] | Highcharts.DataPoint;


export class Chart {
  private chartReference = new AsyncSubject<Highcharts.ChartObject>();
  ref$ = this.chartReference.asObservable();
  ref: Highcharts.ChartObject;

  constructor( private options: Highcharts.Options = { series: []}) {}

  init (el: ElementRef): void {
    Highcharts.chart(el.nativeElement, this.options, chart => {
      this.chartReference.next(chart);
      this.ref = chart;
      this.chartReference.complete();
    });
  }

  destroy () {
    if (this.ref) {
      this.options = this.ref.options;
      this.ref.destroy();
      this.ref = undefined;

      this.chartReference = new AsyncSubject();
      this.ref$ = this.chartReference.asObservable();
    }
  }


  addSeries (config: Highcharts.SeriesOptions, redraw: boolean = true, animation: boolean = false): void {
    this.ref$.subscribe(chart => {
      chart.addSeries(config, redraw, animation);
    });
  }

  addPoint(point: Point, series: number = 0, redraw: boolean = true): void {
    this.ref$.subscribe(chart => {
      if (chart.series.length > series) {
        chart.series[series].addPoint(point, redraw);
      }
    });
  }


  removePoint (point: number, series: number): void {
    this.ref$.subscribe(chart => {
      if (chart.series.length > series && chart.series[series].data.length > point) {
        chart.series[series].removePoint(point, true);
      }
    });
  }

  removeSeries (series: number): void {
    this.ref$.subscribe(chart => {
      if (chart.series.length > series) {
        chart.series[series].remove(true);
      }
    });
  }
}
