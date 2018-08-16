import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { DashboardService } from '../../providers/dashboard.service';
import { ChartType } from '../../models/_enum';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'chart-select',
  templateUrl: './chart-select.component.html',
  styleUrls: ['../dashboard.scss']
})
export class ChartSelectComponent implements OnInit {
  options;
  form: FormGroup;

  constructor(private fb: FormBuilder, private dash: DashboardService) {}

  ngOnInit() {
    this.enumerateOptions();
    let v;
    this.dash.currentChart.subscribe(z => v = z);
    this.form = this.fb.group({
      select: new FormControl(v)
    });
  }

  enumerateOptions = () => {
    const keys = Object.keys(ChartType);
    this.options = keys.slice(keys.length / 2);
  }

  get select() {return this.form.get('select'); }

  selectChart = () => {
    console.log('testing')

    this.dash.setCHART_TYPE(this.select.value);
    this.dash.getNewChartData();
  }

}
