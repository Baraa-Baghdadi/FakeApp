import type { OrderType } from './order-type.enum';
import type { PaymentMethodTypes } from './payment-method-types.enum';
import type { EntityDto } from '@abp/ng.core';
import type { OrderItemType } from './order-item-type.enum';

export interface CreateOrderDto {
  patientId?: string;
  hasInsurance: boolean;
  currency?: string;
  decimals?: number;
  orderType: OrderType;
  paymentMethodTypes: PaymentMethodTypes;
  deliveryDate?: string;
  deliveryCost?: number;
  addressId?: string;
  note?: string;
  instruction?: string;
  items: OrderItemDto[];
}

export interface OrderDto extends EntityDto<string> {
  orderId?: string;
  hasInsurance: boolean;
  total: number;
  currency?: string;
  decimals?: number;
  orderType: OrderType;
  paymentMethodTypes: PaymentMethodTypes;
  cretedOn: number;
  deliveryDate?: string;
  deliveryCost?: number;
  addressId?: string;
  note?: string;
  instruction?: string;
  items: OrderItemDto[];
}

export interface OrderItemDto {
  quantity: number;
  price: number;
  instruction?: string;
  isOtc: boolean;
  orginalId?: string;
  type: OrderItemType;
  name?: string;
  categoryName?: string;
}
