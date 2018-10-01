import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'help-container',
  templateUrl: './help-container.component.html',
  styleUrls: ['./help-container.component.scss']
})
export class HelpContainerComponent implements OnInit {
  public displaying;
  public faqs = 'item';
  public contact = 'item';
  public terms = 'item';
  ngOnInit() {}
  select (tab: string) {
    switch (tab) {
      case 'faqs':
        this.faqs = 'item active';
        this.contact = 'item ';
        this.terms = 'item ';
        this.displaying = 'faqs';
      break;
      case 'contact':
        this.faqs = 'item ';
        this.contact = 'item active';
        this.terms = 'item ';
        this.displaying = 'contact';
      break;
      case 'terms':
        this.faqs = 'item ';
        this.contact = 'item ';
        this.terms = 'item active';
        this.displaying = 'terms';
      break;
      default: return false;
    }
  }
}
