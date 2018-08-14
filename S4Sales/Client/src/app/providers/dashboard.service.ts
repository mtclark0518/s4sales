import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, AsyncSubject } from 'rxjs';
import { ChartService } from './chart.service';
import { FDOT_AGENCIES } from '../models/fdot';
import { COUNTIES } from '../models/county.enum';


export class Overview {
  name: string;
  filter_state?: number;
  total_reports: number;
  total_revenue: number;
  total_reimbursed: number;
}
export enum FilterState {
  State,
  County,
  Agency
}

export enum DateFilter {
  Month,
  Year
}
export enum ChartType {
  Reporting,
  Reimbursing
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  domain = 'http://localhost:5000/api/data/';

  private CurrentOverview = new BehaviorSubject<Overview>(null);
  public currentOverview = this.CurrentOverview.asObservable();

  private CurrentFilterState = new BehaviorSubject<FilterState>(FilterState.State);
  public currentFilterState = this.CurrentFilterState.asObservable();

  private SelectedChart = new BehaviorSubject<ChartType>(ChartType.Reimbursing);
  public selectedChart = this.SelectedChart.asObservable();

  private AGENCY = new BehaviorSubject<FDOT_AGENCIES>(null);
  public fdotAgency = this.AGENCY.asObservable();

  private COUNTY = new BehaviorSubject<COUNTIES>(null);
  public county = this.COUNTY.asObservable();

  private DATE_FILTER = new BehaviorSubject<DateFilter>(DateFilter.Year);
  public dateFilter = this.DATE_FILTER.asObservable();

  private DATE_VALUE = new BehaviorSubject<number>(2018);
  public dateValue = this.DATE_VALUE.asObservable();

  constructor(private http: HttpClient, private chart: ChartService) { }
  // accesssor methods to update dashboard values
  public setAGENCY = value => this.AGENCY.next(value);
  public setCOUNTY = value => this.COUNTY.next(value);
  public setDATE_FILTER = value => this.DATE_FILTER.next(value);
  public setDATE_VALUE = value => this.DATE_VALUE.next(value);
  public setFilterState = value => this.CurrentFilterState.next(value);

  public getNewChartData(): void {
    let filter, value, name, date_filter, date_value;

    this.dateFilter.subscribe(df => date_filter = df);
    this.dateValue.subscribe(dv => date_value = dv);

    this.currentFilterState.subscribe(f => filter = f);
    if (filter === FilterState.County) {
      this.county.subscribe(v => value = v);
    }
    if (filter === FilterState.Agency) {
      this.fdotAgency.subscribe(v => value = v);
    }
    if (filter === FilterState.State) {
      value = '*';
    }

    this.selectedChart.subscribe(c => name = c);
    name === ChartType.Reimbursing ?
      this.getReimbursementChart(filter, value, date_filter, date_value) :
      this.getReportingChart(filter, value, date_filter, date_value);
  }

  private getReportingChart(a, b, c, d ) {
    const headers = new HttpHeaders({
      chart_type: a,
      ct_value: b,
      data_type: c,
      dt_value:  d
    });

    this.http.get(this.domain + 'reporting', {headers})
      .subscribe(res =>
        this.setChart(res));
  }

  private getReimbursementChart(a, b, c, d) {
    const headers = new HttpHeaders({
      chart_type: a,
      ct_value: b,
      data_type: c,
      dt_value: d
    });

    this.http.get(this.domain + 'reimbursement', {headers})
    .subscribe(res =>
      this.setChart(res));
  }


  getReportIndex() {
    this.http.get(this.domain + 'chart')
      .subscribe( response => this.formatOverview(response));
  }
  public formatOverview(data) {
    let filter, chart_name;
    this.currentFilterState.subscribe(f => filter = f);
    this.selectedChart.subscribe(c => chart_name = c);
    const report = new Overview();
    report.name = chart_name;
    report.total_reports = data.length;
    report.total_revenue = data.length * 16;
    report.total_reimbursed = 0;
    data.forEach((d => {
      if (d.timely) {
        report.total_reimbursed += 5;
      }
    }));
    this.setOverview(report);
  }
  public setOverview = (value) => {
    this.CurrentOverview.next(value);
    this.chart.setChartOptions(value);
  }

  public setChart (data) {
    console.log(data);
  }




}
