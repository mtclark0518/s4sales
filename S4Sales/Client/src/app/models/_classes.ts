import { AccountRequestType, Action } from './_enums';

export class AccountProfile {
  first_name?: string;
  last_name?: number;
  email?: string;
}
export class S4Request {
  email: string;
  first_name: string;
  last_name: string;
  organization?: string;
  request_type: AccountRequestType;
  user_name: string;
}
export class Credentials {
  user_name?: string;
  email?: string;
  password: string;
}
export class Log {
  action?: Action;
  target?: string;
}
export class Transaction {
  first_name?: string;
  last_name?: string;
  amount: number;
  token: string;
  cart_id: string;
}
export class Overview {
  name: string;
  filter_state?: number;
  total_count?: number;
  total_revenue?: number;
  total_reimbursed?: number;
  month_count?: any;
  agencies?: any;
  chart?: string;
  details?: any;
}
