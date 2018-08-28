import { ReasonsForQuery } from './_enums';

export interface CrashEvent {
  hsmv_report_number: number;
  city_of_crash?: string;
  county_of_crash?: string;
  reporting_agency?: string;
  isClicked?: boolean;
}
export interface Password {
  oldPassword?: string;
  newPassword: string;
}

export interface SearchQuery {
  crashDate?: string;
  reportNumber?: string;
  vehicleVIN?: string;
  participant?: string;
  queryType: string;
  reasonForQuery: ReasonsForQuery;
}
