import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ProviderWorkDay } from './provider-work-day.enum';

export interface GetProviderInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  email?: string;
  pharmacyName?: string;
  pharmacyPhone?: string;
}

export interface ProviderAddressDto extends EntityDto {
  latitude: number;
  longitude: number;
  address?: string;
  cityId: number;
}

export interface ProviderDto {
  id?: string;
  tenantId?: string;
  email?: string;
  pharmacyName?: string;
  pharmacyPhone?: string;
  concurrencyStamp?: string;
  workingTimes: WorkingTimeDto[];
}

export interface ProviderUpdateDto {
  concurrencyStamp?: string;
  workingTimes: WorkingTimeDto[];
}

export interface WorkingTimeDto extends EntityDto<string> {
  providerId?: string;
  workDay: ProviderWorkDay;
  from?: string;
  to?: string;
}

export interface WorkingTimeForMobileDto {
  workDay: ProviderWorkDay;
  from?: string;
  to?: string;
}
