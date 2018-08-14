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
  options;
  form: FormGroup;

  constructor(private fb: FormBuilder, private dash: DashboardService) {}

  ngOnInit() {
    this.enumerateOptions();

    this.form = this.fb.group({
      select: new FormControl(this.options[0])
    });
  }

  enumerateOptions = () => {
    const keys = Object.keys(ChartType);
    this.options = keys.slice(keys.length / 2);
  }

  get select() {return this.form.get('select'); }

  selectChart = () => {
    this.dash.setCHART_TYPE(this.select.value);
  }

}
