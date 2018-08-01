import { Directive, Input, ElementRef, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Chart } from '../models/chart';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[chart]'
})
export class ChartDirective implements OnChanges, OnDestroy, OnInit {
 @Input() chart: Chart;

 constructor( private el: ElementRef) { }

 ngOnInit() {
   this.init();
 }

 ngOnChanges(changes: SimpleChanges) {
   if (!changes.chart.isFirstChange()) {
      this.destroy();
      this.init();
    }
 }

  ngOnDestroy() {
   this.destroy();
 }


 private init() {
   if (this.chart instanceof Chart) {
     this.chart.init(this.el);
    }
  }
  private destroy() {
    if (this.chart instanceof Chart) {
      this.chart.destroy();
    }
  }
}
