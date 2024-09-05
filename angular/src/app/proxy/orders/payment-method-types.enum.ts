import { mapEnumToOptions } from '@abp/ng.core';

export enum PaymentMethodTypes {
  Cash = 0,
  Credit = 1,
}

export const paymentMethodTypesOptions = mapEnumToOptions(PaymentMethodTypes);
