import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { DashboardService } from '../../providers/dashboard.service';
import { FilterState } from '../../models/_enum';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'filter',
  templateUrl: './filter.component.html',
  styleUrls: ['../dashboard.scss']
})
export class FilterComponent implements OnInit {
  options;
  form: FormGroup;

  constructor(private fb: FormBuilder, private dash: DashboardService) { }

  ngOnInit() {
    this.enumerateOptions();
    this.form = this.fb.group({
      filter: new FormControl(this.options[0])
    });
  }

  reset(): void {
    this.filter.setValue(this.options[0]);
    this.update();
  }

  enumerateOptions = () => {
    const keys = Object.keys(FilterState);
    this.options = keys.slice(keys.length / 2);
  }

  get filter() { return this.form.get('filter'); }

  update = () => this.dash.setFILTER_STATE(this.filter.value);

}
