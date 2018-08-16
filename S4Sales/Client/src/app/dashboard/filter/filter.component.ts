import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { DashboardService } from '../../providers/dashboard.service';
import { FilterState } from '../../models/_enum';
import { FDOT_AGENCIES } from '../../models/fdot';
import { COUNTIES } from '../../models/county.enum';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'filter',
  templateUrl: './filter.component.html',
  styleUrls: ['../dashboard.scss']
})
export class FilterComponent implements OnInit {

  public form: FormGroup;
  public fs: FilterState;
  public yr;

  public agencies: string[];
  public counties: string[];
  public filters: string[];

  constructor(private fb: FormBuilder, private dash: DashboardService) { }

  ngOnInit() {
    this.dash.dateValue.subscribe( v => this.yr = v);
    this.dash.currentFilterState.subscribe( v => this.fs = v);
    this.enumerateOptions();
    this.form = this.fb.group({
      filter: new FormControl(FilterState[this.fs]),
      year: new FormControl(this.yr),
      agency: new FormControl(this.agencies[0]),
      county: new FormControl(this.counties[0])
    });
  }
  get filter() { return this.form.get('filter'); }
  get agency() { return this.form.get('agency'); }
  get county() { return this.form.get('county'); }
  get year() { return this.form.get('year'); }

  enumerateOptions = () => {
    let keys;
    keys = Object.keys(FilterState);
    this.filters = keys.slice(keys.length / 2);
    keys = Object.keys(FDOT_AGENCIES);
    this.agencies = keys.slice(keys.length / 2);
    keys = Object.keys(COUNTIES);
    this.counties = keys.slice(keys.length / 2);
  }
  reset(): void {
    this.filter.setValue(this.filters[0]);
    this.year.setValue('2018');
    this.agency.setValue(this.agencies[0]);
    this.county.setValue(this.counties[0]);
    this.update();
  }

  update = () => {
    this.dash.setFILTER_STATE(this.filter.value);
    this.dash.setDATE_VALUE(this.year.value);
    this.dash.setAGENCY(this.agency.value);
    this.dash.setCOUNTY(this.county.value);
  }


  handle() {
    this.dash.getNewChartData();
  }
}
