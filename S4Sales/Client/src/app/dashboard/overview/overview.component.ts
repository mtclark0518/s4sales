import { Component, OnInit } from '@angular/core';
import { DashboardService, Overview } from '../../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'overview',
  templateUrl: './overview.component.html',
  styleUrls: ['../dashboard.scss']
})
export class OverviewComponent implements OnInit {

  constructor( private dash: DashboardService) { }
  overview: Overview;
  ngOnInit() {
    this.dash.currentOverview.subscribe( o => this.overview = o);
  }

}
