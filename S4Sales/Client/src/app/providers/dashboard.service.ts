import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ChartService } from './chart.service';
import { FDOT_AGENCIES } from '../models/fdot';
import { COUNTIES } from '../models/county.enum';
import { formatDate } from '../../../node_modules/@angular/common';
import { FilterState, ChartType, DateFilter } from '../models/_enum';
import { Overview } from '../models/_class';



@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  domain = 'http://localhost:5000/api/data/';

  private CurrentOverview = new BehaviorSubject<Overview>(new Overview());
  public currentOverview = this.CurrentOverview.asObservable();

  private CURRENT_FILTER_STATE = new BehaviorSubject<FilterState>(FilterState.State);
  public currentFilterState = this.CURRENT_FILTER_STATE.asObservable();

  private CURRENT_CHART_TYPE = new BehaviorSubject<ChartType>(ChartType['Select A Chart']);
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
    this.dateFilter.subscribe(df => date_filter = df);
    this.dateValue.subscribe(dv => date_value = dv);
    this.currentFilterState.subscribe(f => filter = FilterState[f]);

    if (filter === FilterState.County) {
      this.county.subscribe(v => value = v);
    }
    if (filter === FilterState.Agency) {
      this.fdotAgency.subscribe(v => value = v);
    }
    if (filter === FilterState.State) {
      value = '*';
    }
    this.currentChart.subscribe(c => name = c);

    console.log(FilterState[filter], value , DateFilter[date_filter], date_value);
    // value of chart-select-component
    name === 'Reimbursements' ?
      this.getReimbursementChart(FilterState[filter], value, DateFilter[date_filter], date_value) :
      this.getReportingChart(FilterState[filter], value, DateFilter[date_filter], date_value);
  }

  private getReportingChart(a, b, c, d ) {
    const headers = new HttpHeaders({
      filter: a,
      filter_lookup: b,
      date_filter: c,
      date_lookup:  d
    });
    this.http.get(this.domain + 'reporting', {headers})
      .subscribe( res =>
        this.setReportingChart(res));
  }

  private getReimbursementChart(a, b, c, d) {
    const headers = new HttpHeaders({
      filter: a,
      filter_lookup: b,
      date_filter: c,
      date_lookup: d
    });
    this.http.get(this.domain + 'reimbursement', {headers})
    .subscribe( res =>
      this.setReimbursementChart(res));
  }

  public setReportingChart (data) {
    const reports_by_month = this.getMonthCount(data, 'crash_date_and_time');
    const reports_by_agency = this.getAgencyCount(data, 'reporting_agency');

    const overview = new Overview();
    overview.name = 'Reports by Month';
    overview.total_reports = data.length;
    overview.agencies = reports_by_agency;
    overview.month_count = reports_by_month;
    overview.chart = 'line';
    overview['details'] = {...this.currentSettings()};
    this.chart.setChartOptions(overview);

  }




  public setReimbursementChart (data) {
    let amount = 0;
    data.forEach( x => { if (x.timely) {  amount += 5; } } );
    const sales_per_month = this.getMonthCount(data, 'reimbursement_date');
    const overview = new Overview();

    overview.name = 'Monthly Sales';
    overview.total_reports = data.length;
    overview.month_count = sales_per_month;
    overview.chart = 'column';
    overview.total_revenue = overview.total_reports * 16;
    overview.total_reimbursed = amount;
    overview['details'] = {...this.currentSettings()};
    this.chart.setChartOptions(overview);
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


currentSettings(): Object {
  let v1, v2, v3, v4;
  this.currentChart.subscribe(x1 => v1 = ChartType[x1]);
  this.currentFilterState.subscribe(x2 => v2 = FilterState[x2]);
  this.dateFilter.subscribe(x3 => v3 = DateFilter[x3]);
  this.dateValue.subscribe(x4 => v4 = x4);
  return {
    chart_name: v1,
    filter: v2,
    date_filter: v3,
    date_value : v4
  };
}



}


