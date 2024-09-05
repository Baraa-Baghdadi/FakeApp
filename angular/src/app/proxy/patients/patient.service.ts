import type { GetPatientInput, PatientProviderDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  apiName = 'Default';
  

  getAllPatientsOfProvider = (input: GetPatientInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PatientProviderDto>>({
      method: 'GET',
      url: '/api/app/patient/patients-of-provider',
      params: { filterText: input.filterText, mobileNumber: input.mobileNumber, countryCode: input.countryCode, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPatientsOfProvider = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientProviderDto>({
      method: 'GET',
      url: `/api/app/patient/${id}/patients-of-provider`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
