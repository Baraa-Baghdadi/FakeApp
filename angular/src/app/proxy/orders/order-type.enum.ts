import { mapEnumToOptions } from '@abp/ng.core';

export enum OrderType {
  PickUp = 0,
  Delivery = 1,
}

export const orderTypeOptions = mapEnumToOptions(OrderType);
