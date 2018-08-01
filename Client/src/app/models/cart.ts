import { CrashEvent } from './crash-event';

export class Cart {
  public items: CrashEvent[] = new Array<CrashEvent>();
  public itemCount = 0;
  public cost = 0;

  public updateCart(current: Cart) {
    this.items = current.items;
    this.itemCount = current.items.length;
    this.cost = current.cost;
  }
}
