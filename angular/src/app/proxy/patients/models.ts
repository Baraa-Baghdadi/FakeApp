import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { PatientAddingType } from './patient-adding-type.enum';
import type { PatientAddressType } from '../enums/patient-address-type.enum';

export interface GetPatientInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  mobileNumber?: string;
  countryCode?: string;
}

export interface PatientProviderDto extends EntityDto<string> {
  patientId?: string;
  providerId?: string;
  addingDate?: string;
  mobileNumber?: string;
  fullMobileNumber?: string;
  countryCode?: string;
  name?: string;
  dob?: string;
  addingType: PatientAddingType;
}

export interface GetPatientAddressInput extends PagedAndSortedResultRequestDto {
  name?: string;
  buildingName?: string;
  appartmentNumber?: string;
  landMark?: string;
  longitude?: string;
  latitude?: string;
  address?: string;
  type?: PatientAddressType;
}

export interface PatientAddressCreateDto {
  name?: string;
  buildingName?: string;
  appartmentNumber?: string;
  landMark?: string;
  longitude?: string;
  latitude?: string;
  address?: string;
  type: PatientAddressType;
  isDefault: boolean;
}

export interface PatientAddressDto extends EntityDto<string> {
  patientId?: string;
  name?: string;
  buildingName?: string;
  appartmentNumber?: string;
  landMark?: string;
  longitude?: string;
  latitude?: string;
  address?: string;
  type: PatientAddressType;
  isDefault: boolean;
}

export interface PatientAddressUpdateDto extends EntityDto<string> {
  name?: string;
  buildingName?: string;
  appartmentNumber?: string;
  landMark?: string;
  longitude?: string;
  latitude?: string;
  address?: string;
  isDefault: boolean;
  type?: PatientAddressType;
}

export interface PatientProviderCreateDto {
  providerId?: string;
  addingType: PatientAddingType;
}
