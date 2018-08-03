import { Component, OnInit } from '@angular/core';
import { Concern } from './concern.enum';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {
  public concerns: string[];
  public Contact = {
    firstName: '',
    lastName: '',
    email: '',
    confirmed: false,
    concern: 'What can we help you with?',
    comment: ''
  };
  ngOnInit() {
    const concerns = Object.keys(Concern);
    this.concerns = concerns.slice(concerns.length / 2);
  }
  submitContact() {
    console.log(this.Contact);
  }

}
