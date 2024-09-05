import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { Response } from '../../api-response/models';
import type { ProviderDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ProviderMobileService {
  apiName = 'Default';
  

  scanQRByProviderCode = (providerCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<ProviderDto>>({
      method: 'POST',
      url: '/api/app/provider-mobile/scan-qR',
      params: { providerCode },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
