import { AccountRequestType, Action } from './_enums';
import { Password } from './_interfaces';

export class AgencyAccount {
  agency_id?: string;
  agency?: string;
  first_name?: string;
  last_name?: number;
  active?: boolean;
  email?: string;
}
export class NewAgency {
  agency: string;
  email: string;
  first_name: string;
  last_name: string;
  password: string;
}
export class OnboardingDetails {
  agency: string;
  token: string;
}
export class Credentials {
  agency_name?: string;
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
  email: string;
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
