import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ChartService } from './chart.service';


export class Overview {
  name: string;
  filter_state?: number;
  total_reports: number;
  total_revenue: number;
  total_reimbursed: number;

}


@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  domain = 'http://localhost:5000/api/data/';

  private CurrentOverview = new BehaviorSubject<Overview>(null);
  private CurrentFilterState = new BehaviorSubject<number>(0);
  public currentOverview = this.CurrentOverview.asObservable();
  public currentFilterState = this.CurrentFilterState.asObservable();
  private SelectedChart = new BehaviorSubject<string>('Report Sales');
  public selectedChart = this.SelectedChart.asObservable();

  constructor(private http: HttpClient, private chart: ChartService) { }

  // applyFilter() {// TODO}
  getReportIndex() {
    this.http.get(this.domain + 'chart')
      .subscribe( response => this.formatOverview(response));
  }
  formatOverview(data) {
    console.log(data);
    let filter, chart_name;
    this.currentFilterState.subscribe(f => filter = f);
    this.selectedChart.subscribe(c => chart_name = c);

    const report = new Overview();
    report.name = chart_name;
    report.total_reports = data.length;
    report.total_revenue = data.length * 16;
    report.total_reimbursed = 0;
    data.forEach((d => {
      if (d.incentivized) {
        report.total_reimbursed += 5;
      }
    }));

    this.setOverview(report);
  }

  // getOverview(name: string) {
  //   // let filter;
  //   // this.currentFilterState.subscribe(f => filter = f);
  //   // const TempData = new Overview();
  //   //   TempData.name = name;
  //   //   TempData.total_reports = parseInt(name, 8) * 10;
  //   //   TempData.filter_state = filter;
  //   //   this.setOverview(TempData);
  //   // let headers: HttpHeaders;
  //   //   headers = new HttpHeaders({
  //   //     'Accept': 'application/json',
  //   //     'Content-Type': 'application/json',
  //   //     'chart': name
  //   //   });
  //   //   this.http.get(this.domain + '/logging/chart', {headers: headers})
  //   //     .subscribe(response => {
  //   //       this.setOverview(response);
  //   //   });
  // }

  public setChart (name: string) {
    // this.getOverview(name);
  }

  public setFilterState (filter) {
    // this.applyFilter();
    this.CurrentFilterState.next(filter);
  }

  private setOverview (overview) {
    this.CurrentOverview.next(overview);
    this.chart.setChartOptions(overview);
  }


}
