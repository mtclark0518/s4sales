import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { DashboardService, ChartType } from '../../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'chart-select',
  templateUrl: './chart-select.component.html',
  styleUrls: ['../dashboard.scss']
})
export class ChartSelectComponent implements OnInit {

  options: ChartType;
  form: FormGroup;

  constructor(private fb: FormBuilder, private dash: DashboardService) {}

  ngOnInit() {
    this.form = this.fb.group({
      select: new FormControl(this.options[0])
    });
    this.selectChart();
  }

  get select() {return this.form.get('select'); }

  selectChart() {
    const choice = this.select.value;
    this.dash.setChart(choice);
  }
}
