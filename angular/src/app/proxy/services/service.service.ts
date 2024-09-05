import type { CreateServiceDto, GetServieInput, ServiceDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ServiceService {
  apiName = 'Default';
  

  createService = (input: CreateServiceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ServiceDto>({
      method: 'POST',
      url: '/api/app/service/service',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getListAyncByInput = (input: GetServieInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ServiceDto>>({
      method: 'GET',
      url: '/api/app/service/aync',
      params: { filterText: input.filterText, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getService = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ServiceDto>({
      method: 'GET',
      url: `/api/app/service/${id}/service`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateServiceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ServiceDto>({
      method: 'PUT',
      url: `/api/app/service/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
