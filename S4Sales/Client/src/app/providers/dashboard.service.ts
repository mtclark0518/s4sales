import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ChartService } from './chart.service';
import { FDOT_AGENCIES } from '../models/fdot';
import { COUNTIES } from '../models/county.enum';
import { formatDate } from '../../../node_modules/@angular/common';


export class Overview {
  name: string;
  filter_state?: number;
  total_reports?: number;
  total_revenue?: number;
  total_reimbursed?: number;
  count?: any;
  agencies?: any;
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
  Reports,
  Reimbursements
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  domain = 'http://localhost:5000/api/data/';

  private CurrentOverview = new BehaviorSubject<Overview>(new Overview());
  public currentOverview = this.CurrentOverview.asObservable();

  private CURRENT_FILTER_STATE = new BehaviorSubject<FilterState>(FilterState.State);
  public currentFilterState = this.CURRENT_FILTER_STATE.asObservable();

  private CURRENT_CHART_TYPE = new BehaviorSubject<ChartType>(ChartType.Reports);
  public currentChart = this.CURRENT_CHART_TYPE.asObservable();

  private AGENCY = new BehaviorSubject<FDOT_AGENCIES>(null);
  public fdotAgency = this.AGENCY.asObservable();

  private COUNTY = new BehaviorSubject<COUNTIES>(null);
  public county = this.COUNTY.asObservable();

  private DATE_FILTER = new BehaviorSubject<DateFilter>(DateFilter.Year);
  public dateFilter = this.DATE_FILTER.asObservable();

  private DATE_VALUE = new BehaviorSubject<string>('2018');
  public dateValue = this.DATE_VALUE.asObservable();

  constructor(private http: HttpClient, private chart: ChartService) { }
  // accesssor methods to update dashboard values
  public setAGENCY = value => this.AGENCY.next(value);
  public setCHART_TYPE = value => this.CURRENT_CHART_TYPE.next(value);
  public setCOUNTY = value => this.COUNTY.next(value);
  public setDATE_FILTER = value => this.DATE_FILTER.next(value);
  public setDATE_VALUE = value => this.DATE_VALUE.next(value);
  public setFILTER_STATE = value => this.CURRENT_FILTER_STATE.next(value);


  public getNewChartData(): void {
    // variable definitions
    let filter, value, name, date_filter, date_value;
    // value of filter component
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
    // value of chart-select-component
    this.currentChart.subscribe(c => name = c);

    name === ChartType.Reimbursements ?
      this.getReimbursementChart(FilterState[filter], value, DateFilter[date_filter], date_value) :
      this.getReportingChart(FilterState[filter], value, DateFilter[date_filter], date_value);
  }

  private getReportingChart(a, b, c, d ) {
    const headers = new HttpHeaders({
      chart_type: a,
      ct_value: b,
      data_type: c,
      dt_value:  d
    });

    this.http.get(this.domain + 'reporting', {headers})
      .subscribe( res =>
        this.setReportingChart(res));
  }

  private getReimbursementChart(a, b, c, d) {
    const headers = new HttpHeaders({
      chart_type: a,
      ct_value: b,
      data_type: c,
      dt_value: d
    });

    this.http.get(this.domain + 'reimbursement', {headers})
    .subscribe( res =>
      this.setReimbursementChart(res));
  }

  public setReportingChart (data) {
    const reports_by_month = this.getMonthCount(data, 'crash_date_and_time');
    const reports_by_agency = this.getAgencyCount(data, 'reporting_agency');
    console.log(reports_by_agency, reports_by_month);
    const overview = new Overview();
    overview.name = 'Reports by Month';
    overview.total_reports = data.length;
    overview.agencies = reports_by_agency;
    overview.count = reports_by_month;
    this.chart.setChartOptions(overview);
    console.log(overview);
  }

  public setReimbursementChart (data) {
    const count = this.getMonthCount(data, 'reimbursement_date');
    const agencies = this.getAgencyCount(data, 'reporting_agency');
  }

  // gets incoming items and
  private getMonthCount(dt: Array<any>, param: string) {
    const count = {};
    dt.forEach( d => {
      const date = d[param];
      const month = formatDate(date, 'short', 'en-US', 'UTC').split('', 1)[0];
      if(count[month]) {
        count[month]++;
      } else {
        count[month] = 0;
        count[month]++;
      }
    });
    return count;
  }

  // gets incoming items and
  private getAgencyCount(dt: Array<any>, param: string) {
    const agencies = {};
    dt.forEach( d => {
      const agency = d[param];
      if(agencies[agency]) {
        agencies[agency]++;
      } else {
        agencies[agency] = 0;
        agencies[agency]++;
      }
    });
    return agencies;
  }






  getReportIndex() {
    this.http.get(this.domain + 'index')
      .subscribe( response => this.formatOverview(response));
  }

  public formatOverview(data) {
    let filter, chart_name;

    this.currentFilterState.subscribe(f => filter =  FilterState[f]);
    this.currentChart.subscribe(c => chart_name = ChartType[c]);

    const overview = new Overview();
    overview.name = chart_name;
    overview.total_reports = data.length;
    overview.total_revenue = data.length * 16;
    overview.total_reimbursed = 0;

    data.forEach( d => {
      if (d.timely) {
        overview.total_reimbursed += 5;
      }
    });
    this.CurrentOverview.next(overview);
    this.chart.setChartOptions(overview);
  }



}


