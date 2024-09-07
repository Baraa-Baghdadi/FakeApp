import type { ProviderNotificationDto } from './models';
import type { NotificationTypeEnum } from './notification-type-enum.enum';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationProviderService {
  apiName = 'Default';
  

  createAddedYouToMyPharmacytNotificationByIdAndContentAndTypeAndExtraproperties = (id: string, content: string, type: NotificationTypeEnum, extraproperties: Record<string, string>, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notification-provider/${id}/added-you-to-my-pharmacyt-notification`,
      params: { content, type },
      body: extraproperties,
    },
    { apiName: this.apiName,...config });
  

  getCountOfUnreadingMsg = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/app/notification-provider/count-of-unreading-msg',
    },
    { apiName: this.apiName,...config });
  

  getListOfProviderNotificationByInput = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProviderNotificationDto>>({
      method: 'GET',
      url: '/api/app/notification-provider/of-provider-notification',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  markAllAsRead = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/notification-provider/mark-all-as-read',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
