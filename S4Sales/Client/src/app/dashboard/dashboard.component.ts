import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.scss']
})
export class DashboardComponent implements OnInit {
  constructor( private dash: DashboardService) {

  }

  ngOnInit() {
    this.dash.getNewChartData();
  }

}
