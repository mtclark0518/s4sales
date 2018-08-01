import { Component, Input } from '@angular/core';
import { CrashEvent } from '../../models/crash-event';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'crash-event',
  templateUrl: './crash-event.component.html',
  styleUrls: ['./crash-event.component.scss']
})
export class CrashEventComponent {
  @Input('report') report: CrashEvent;
}
