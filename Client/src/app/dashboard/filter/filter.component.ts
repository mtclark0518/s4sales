import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { DashboardService } from '../../providers/dashboard.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'filter',
  templateUrl: './filter.component.html',
  styleUrls: ['../dashboard.scss']
})
export class FilterComponent implements OnInit {
  form: FormGroup;
  constructor(private fb: FormBuilder, private dash: DashboardService) { }

  ngOnInit() {
    this.form = this.fb.group({
      filter: new FormControl('')
    });
  }

  clear(): void {
    this.filter.setValue('');
    this.update();
  }

  get filter() { return this.form.get('filter'); }


  update() {
    this.dash.setFilterState(this.filter.value);
  }
}
