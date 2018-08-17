// tslint:disable:max-line-length

export enum FilterState {
  State,
  County,
  Agency
}

export enum DateFilter {
  Month,
  Year
}

export enum ChartType {
  'Select A Chart',
  Reports,
  Reimbursements
}

export enum Month {
  January,
  February,
  March,
  April,
  May,
  June,
  July,
  August,
  September,
  October,
  November,
  December
}

export enum Action {
  Add,
  Remove,
  Clear,
  Checkout
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


export enum ReasonsForQuery {
  'I am a party involved in the accident',
  'I am a legal representative to a party involved in the accident',
  'I am a licensed insurance agent to a party involved in the accident, their insurer or insurers to which they applied for insurance coverage',
  'I am a person under contract to provide claims or underwriting information to a qualifying insurance company',
  'I am a prosecuting authority',
  'I represent a radio or television station licensed by the FCC or newspaper qualified to publish legal notices or a free newspaper or general circulation, which qualifies under the statute',
  'I represent a local, state or federal agency that is authorized by law to have access to these reports',
  'I represent a Victim Service Program, as defined in §316.003(84), Florida Statutes',
  'I am an interested party'
}
