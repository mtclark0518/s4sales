import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.scss']
})
export class RedirectComponent implements OnInit {

  constructor( private router: Router) { }

  ngOnInit() {
    setTimeout(() => {
      alert('you da man!');
      this.router.navigateByUrl('/');
    }, 10000);
  }

}
