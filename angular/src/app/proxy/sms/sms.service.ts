import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SmsService {
  apiName = 'Default';
  

  sendSmsMessageByMobileNumberAndMessage = (mobileNumber: string, message: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/sms/send-sms-message',
      params: { mobileNumber, message },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
