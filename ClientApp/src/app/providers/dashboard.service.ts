import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ChartService } from './chart.service';


export class Overview {
  name: string;
  total: number;
  filter_state?: number;
}


@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  domain = 'http://localhost:5000/api';

  private CurrentOverview = new BehaviorSubject<Overview>(null);
  private CurrentFilterState = new BehaviorSubject<number>(0);
  public currentOverview = this.CurrentOverview.asObservable();
  public currentFilterState = this.CurrentFilterState.asObservable();

  constructor(private http: HttpClient, private chart: ChartService) { }

  // applyFilter() {// TODO}

  getOverview(name: string) {

    let filter;
    this.currentFilterState.subscribe(f => filter = f);

    const TempData = new Overview();

      TempData.name = name;
      TempData.total = parseInt(name, 8) * 10;
      TempData.filter_state = filter;

      this.setOverview(TempData);


    // let headers: HttpHeaders;
    //   headers = new HttpHeaders({
    //     'Accept': 'application/json',
    //     'Content-Type': 'application/json',
    //     'chart': name
    //   });

    //   this.http.get(this.domain + '/logging/chart', {headers: headers})
    //     .subscribe(response => {
    //       this.setOverview(response);
    //   });
  }

  public setChart (name: string) {
    this.getOverview(name);
  }

  public setChartOptions(overview: Overview) {
    const chart_data = {};
    this.chart.setChartOptions(chart_data);
  }

  public setFilterState (filter) {
    // this.applyFilter();
    this.CurrentFilterState.next(filter);
  }

  private setOverview (overview: Overview) {
    this.CurrentOverview.next(overview);
    this.setChartOptions(overview);
  }


}
