import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


export class Modal {
  id: string;
  content: any;
}
@Injectable({
  providedIn: 'root'
})

export class ModalService {

  private ACTIVE = new BehaviorSubject<boolean>(false);
  public active = this.ACTIVE.asObservable();

  private ACTIVE_MODAL = new BehaviorSubject<string>('modal');
  public active_modal = this.ACTIVE_MODAL.asObservable();



  open(name: string) {
    // open modal specified by id
    this.ACTIVE_MODAL.next(name);
    this.ACTIVE.next(true);
  }

  close() {
    this.ACTIVE_MODAL.next(null);
    this.ACTIVE.next(false);
  }
}
