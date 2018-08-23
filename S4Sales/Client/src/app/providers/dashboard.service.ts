import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ChartService } from './chart.service';
import { FDOT_AGENCIES } from '../models/fdot.enum';
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

    private DISPLAYING = new BehaviorSubject<string>('summary');
    public displaying = this.DISPLAYING.asObservable();


    constructor(private http: HttpClient, private chart: ChartService) { }


    // accesssor methods to update dashboard values
    public setAGENCY = value => this.AGENCY.next(value);
    public setCHART_TYPE = value => this.CURRENT_CHART_TYPE.next(value);
    public setCOUNTY = value => this.COUNTY.next(value);
    public setDATE_FILTER = value => this.DATE_FILTER.next(value);
    public setDATE_VALUE = value => this.DATE_VALUE.next(value);
    public setFILTER_STATE = value => this.CURRENT_FILTER_STATE.next(value);
    public setDISPLAYING = value => this.DISPLAYING.next(value);


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

        // value of chart-select-component
        this.getChartData(FilterState[filter], value, DateFilter[date_filter], date_value, name);
    }

    public generateReport(from, to) {
      console.log(from, to);
    }


    private getChartData(a, b, c, d, path ) {
        const headers = new HttpHeaders({
            filter: a,
            filter_lookup: b,
            date_filter: c,
            date_lookup:  d
        });
        this.http.get(this.domain + path, {headers})
            .subscribe( res => {
              this.setChartData(res);
            });
    }



    private setChartData = (data) => {

      const chartOptions = new Overview();
      chartOptions['total_count'] = data.length;
      const _chart_type = ChartType[this.currentSettings()['chart_type']];
      chartOptions['month_count'] =  _chart_type === 'Reporting' ?
        this.getItemCount(data, 'crash_date_and_time') : this.getItemCount(data, 'reimbursement_date');

      chartOptions['details'] = this.currentSettings();

      let timely = data.filter(item => item['timely']);
      timely = this.reduce(timely, 'reimbursement_amount');

      chartOptions['total_reimbursed'] = timely;

      this.chart.setChartOptions(chartOptions);
    }

    private currentSettings(): Object {
      let v1, v2, v3, v4;
      this.currentChart       .subscribe  (x => v1 = ChartType[x]);
      this.currentFilterState .subscribe  (x => v2 = FilterState[x]);
      this.dateFilter         .subscribe  (x => v3 = DateFilter[x]);
      this.dateValue          .subscribe  (x => v4 = x);
      return {
        chart_type:   v1,
        filter:       v2,
        date_filter:  v3,
        date_lookup : v4
      };
    }

    private getItemCount(dt: Array<any>, param: string) {
        const items = {};
        switch ( param ) {
            case 'crash_date_and_time':
            dt.forEach( d => {
                const month = formatDate(d[param], 'short', 'en-US', 'UTC').split('', 1)[0];
                if (items[month]) {
                  items[month]++;
                } else {
                    items[month] = 1;
                }
            });
            break;
            case 'reimbursement_date':
            dt.forEach( d => {
                const month = formatDate(d[param], 'short', 'en-US', 'UTC').split('', 1)[0];
                if (items[month]) {
                  items[month]++;
                } else {
                    items[month] = 1;
                }
            });
            break;
            default:
                dt.forEach( d => {
                    if (items[d[param]]) {
                        items[d[param]]++;
                    } else {
                        items[d[param]] = 1;
                    }
            });
        }
        return items;
    }

    private reduce = (arr, par) => {
      return arr.reduce((ac, cv) => ac + cv[par], 0);
    }
}


