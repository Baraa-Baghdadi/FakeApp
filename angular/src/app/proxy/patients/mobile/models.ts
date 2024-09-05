import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPatientOfProviderForMobile extends PagedAndSortedResultRequestDto {
  filterText?: string;
  pharmacyName?: string;
  pharmacyPhone?: string;
}
