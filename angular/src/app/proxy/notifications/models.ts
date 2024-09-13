import type { FullAuditedEntityDto } from '@abp/ng.core';
import type { NotificationTypeEnum } from './notification-type-enum.enum';

export interface ProviderNotificationDto extends FullAuditedEntityDto<string> {
  entityId?: string;
  title?: string;
  content?: string;
  type: NotificationTypeEnum;
  isRead?: boolean;
  createdOn?: string;
}
