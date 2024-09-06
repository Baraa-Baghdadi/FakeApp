import { mapEnumToOptions } from '@abp/ng.core';

export enum NotificationTypeEnum {
  Order = 0,
  NewPatient = 1,
}

export const notificationTypeEnumOptions = mapEnumToOptions(NotificationTypeEnum);
