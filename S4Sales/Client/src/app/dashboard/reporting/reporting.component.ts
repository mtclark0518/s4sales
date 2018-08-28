import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../providers/dashboard.service';

@Component({
  selector: 'reporting',
  templateUrl: './reporting.component.html',
  styleUrls: ['../dashboard.scss']
})
export class ReportingComponent implements OnInit {
  public items = [];
  constructor(private dash: DashboardService) { }
  ngOnInit() {
    console.log('init');
    this.dash.reportDetails.subscribe(tails_ => this.items = tails_);
  }

}
