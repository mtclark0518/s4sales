import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'tabs',
  templateUrl: './switch-tabs.html',
  styleUrls: ['../dashboard.scss']
})
export class SwitchTabsComponent implements OnInit {
  public summary = 'item';
  public reports = 'item';

  constructor(private dash: DashboardService) { }

  ngOnInit() {
    this.select('summary');

  }

  select (tab: string) {
    switch (tab) {
      case 'summary':
        this.summary = 'item active';
        this.reports = 'item ';
        this.dash.setDISPLAYING('summary');
      break;

      case 'reports':
        this.summary = 'item ';
        this.reports = 'item active';
        this.dash.setDISPLAYING('reports');
      break;
      default:
        console.log('default');
    }
  }
}
