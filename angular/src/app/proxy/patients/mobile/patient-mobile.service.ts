import type { GetPatientOfProviderForMobile } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { Response } from '../../api-response/models';
import type { GetPatientAddressInput, PatientAddressCreateDto, PatientAddressDto, PatientAddressUpdateDto, PatientProviderCreateDto } from '../models';
import type { PharmacyInfoDto } from '../../providers/mobile/models';

@Injectable({
  providedIn: 'root',
})
export class PatientMobileService {
  apiName = 'Default';
  

  addToMyPharmacyByInput = (input: PatientProviderCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'POST',
      url: '/api/app/patient-mobile/to-my-pharmacy',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  createAddress = (input: PatientAddressCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PatientAddressDto>>({
      method: 'POST',
      url: '/api/app/patient-mobile/address',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deleteAddressById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'DELETE',
      url: `/api/app/patient-mobile/${id}/address`,
    },
    { apiName: this.apiName,...config });
  

  getAddress = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PatientAddressDto>>({
      method: 'GET',
      url: `/api/app/patient-mobile/${id}/address`,
    },
    { apiName: this.apiName,...config });
  

  getAddressList = (input: GetPatientAddressInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PagedResultDto<PatientAddressDto>>>({
      method: 'GET',
      url: '/api/app/patient-mobile/address-list',
      params: { name: input.name, buildingName: input.buildingName, appartmentNumber: input.appartmentNumber, landMark: input.landMark, longitude: input.longitude, latitude: input.latitude, address: input.address, type: input.type, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyPharmaciesByInput = (input: GetPatientOfProviderForMobile, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PagedResultDto<PharmacyInfoDto>>>({
      method: 'GET',
      url: '/api/app/patient-mobile/my-pharmacies',
      params: { filterText: input.filterText, pharmacyName: input.pharmacyName, pharmacyPhone: input.pharmacyPhone, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getProvider = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PharmacyInfoDto>>({
      method: 'GET',
      url: `/api/app/patient-mobile/${id}/provider`,
    },
    { apiName: this.apiName,...config });
  

  setAddressAsDefaultByAddressId = (addressId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'POST',
      url: `/api/app/patient-mobile/set-address-as-default/${addressId}`,
    },
    { apiName: this.apiName,...config });
  

  updateAddress = (input: PatientAddressUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<PatientAddressDto>>({
      method: 'PUT',
      url: '/api/app/patient-mobile/address',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
