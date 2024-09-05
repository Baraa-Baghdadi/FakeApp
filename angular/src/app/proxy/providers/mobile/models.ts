import type { WorkingTimeForMobileDto } from '../models';

export interface PharmacyInfoDto {
  id?: string;
  name?: string;
  phone?: string;
  longitude: number;
  latitude: number;
  address?: string;
  addingDate?: string;
  workingTimes: WorkingTimeForMobileDto[];
}
