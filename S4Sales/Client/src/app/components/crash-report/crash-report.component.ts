import { Component, Input } from '@angular/core';
import { CrashReport } from '../../models/_interfaces';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'crash-report',
  templateUrl: './crash-report.component.html',
  styleUrls: ['./crash-report.component.scss']
})
export class CrashReportComponent {
  @Input('report') report: CrashReport;
}
