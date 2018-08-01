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


export enum AccountRequestType {
  Administrator,
  Individual,
  Global,
  Member,
  Organization
}

export enum Status {
  Unverified,
  Pending,
  Approved,
  Rejected
}
