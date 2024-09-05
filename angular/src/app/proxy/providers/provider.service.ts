import type { GetProviderInput, ProviderDto, ProviderUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProviderService {
  apiName = 'Default';
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/provider/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProviderDto>({
      method: 'GET',
      url: `/api/app/provider/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetProviderInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProviderDto>>({
      method: 'GET',
      url: '/api/app/provider',
      params: { filterText: input.filterText, email: input.email, pharmacyName: input.pharmacyName, pharmacyPhone: input.pharmacyPhone, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: ProviderUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProviderDto>({
      method: 'PUT',
      url: `/api/app/provider/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
