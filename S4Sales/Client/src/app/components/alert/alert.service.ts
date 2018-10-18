import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  private ActiveMessage = new BehaviorSubject<string>('');
  public message = this.ActiveMessage.asObservable();

  private Displayed = new BehaviorSubject<boolean>(false);
  public isDisplayed = this.Displayed.asObservable();


  // communicates an alert message to the user
  // disappears after 5s
  public setAlert(message: string) {
    this.ActiveMessage.next(message);
    this.display(true);
    setTimeout(() => this.display(false), 5000);
  }

  public display(value: boolean) {
    this.Displayed.next(value);
  }
}
