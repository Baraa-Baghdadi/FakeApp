import { mapEnumToOptions } from '@abp/ng.core';

export enum PatientAddressType {
  Home = 0,
  Work = 1,
}

export const patientAddressTypeOptions = mapEnumToOptions(PatientAddressType);
