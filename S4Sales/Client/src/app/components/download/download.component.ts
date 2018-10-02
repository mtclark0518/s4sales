import { Component, OnInit } from '@angular/core';
import { EcommerceService } from '../../providers/ecommerce.service';

@Component({
  selector: 'download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss']
})
export class DownloadComponent implements OnInit {
  public Downloads;
  constructor(private ec: EcommerceService) { }

  ngOnInit() {
    this.ec.tokens.subscribe(tokens => this.Downloads = tokens);
  }

}
