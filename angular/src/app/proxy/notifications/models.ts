import type { EntityDto } from '@abp/ng.core';

export interface ProviderNotificationDto extends EntityDto<string> {
  entityId?: string;
  title?: string;
  content?: string;
  tenantId?: string;
  createdOn: number;
}
