import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SearchQuery, ReasonsForQuery } from '../../models/search-query';
import { SearchService } from '../../providers/search.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['../search.scss']
})
export class SearchFormComponent implements OnInit {
  public queryType: string;
  public showingQueryReasons = false;
  public reasonsForQuery: string[];
  public searchForm: FormGroup;
  public query_reason;

  constructor( private formBuilder: FormBuilder, private search: SearchService) {}

  ngOnInit() {
    this.search.queryType.subscribe(type => this.queryType = type);
    const connOptKeys = Object.keys(ReasonsForQuery);
    this.reasonsForQuery = connOptKeys.slice(connOptKeys.length / 2);
    this.searchForm = this.formBuilder.group({
      'participant': new FormControl(null),
      'crashDate': new FormControl(null),
      'vehicleVIN': new FormControl(null),
      'reportNumber': new FormControl(null),
    });
  }
  get participant() { return this.searchForm.get('participant'); }
  get crashDate() { return this.searchForm.get('crashDate'); }
  get vehicleVIN() { return this.searchForm.get('vehicleVIN'); }
  get reportNumber() { return this.searchForm.get('reportNumber'); }


  showQueryReasons() {
    this.showingQueryReasons = true;
  }

  updateSelection($event) {
    this.query_reason = $event.target.value;
  }

  submitSearch($event): void {
    $event.preventDefault();
    const query: SearchQuery = {
      participant: this.participant.value,
      crashDate: this.crashDate.value,
      reportNumber: this.reportNumber.value,
      vehicleVIN: this.vehicleVIN.value,
      reasonForQuery: this.query_reason,
      queryType: this.queryType
    };
    this.search.submitSearch(query);
  }
  public startOver() {
    this.search.updateSearchStatus('');
  }
}
