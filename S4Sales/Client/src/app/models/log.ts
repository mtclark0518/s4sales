export class Log {
  action?: Action;
  target?: string;
}

export enum Action {
  Add,
  Remove,
  Clear,
  Checkout
}

