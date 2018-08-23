import { Component, OnInit } from '@angular/core';
import { ModalService } from '../../components/alert-modal/modal.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'sidepanel',
  templateUrl: './side-panel.component.html',
  styleUrls: ['../dashboard.scss']
})
export class SidePanelComponent implements OnInit {
  modal = 'modal';
  constructor( private modalService: ModalService) { }

  ngOnInit() {
  }


  open(name) {
    this.modalService.open(name);
  }


}
