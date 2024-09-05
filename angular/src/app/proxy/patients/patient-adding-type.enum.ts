import { mapEnumToOptions } from '@abp/ng.core';

export enum PatientAddingType {
  Scan = 0,
  Manual = 1,
}

export const patientAddingTypeOptions = mapEnumToOptions(PatientAddingType);
