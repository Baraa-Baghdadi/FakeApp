import { mapEnumToOptions } from '@abp/ng.core';

export enum OrderItemType {
  Product = 0,
  Drug = 1,
}

export const orderItemTypeOptions = mapEnumToOptions(OrderItemType);
