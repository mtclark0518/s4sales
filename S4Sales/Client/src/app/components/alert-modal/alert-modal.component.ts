import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ModalService } from './modal.service';

@Component({
  selector: 'alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.scss']
})

export class AlertModalComponent implements OnInit, OnDestroy {
  @Input() name: string;
  public active: boolean;
  public current: string;
  constructor(private modal: ModalService) {
  }

  ngOnInit(): void {

    this.modal.active.subscribe(a => this.active = a);
    this.modal.active_modal.subscribe(n => this.current = n);

    console.log(this.name, this.current);
  }
  // remove self from modal service when directive is destroyed
  ngOnDestroy(): void {
  }


  // close modal
  close(): void {
      document.body.classList.remove('jw-modal-open');
  }

}



