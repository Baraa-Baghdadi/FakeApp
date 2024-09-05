import type { ProviderRegisterInfoDto, VerifyCodeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProviderAuthService {
  apiName = 'Default';
  

  generateQrAndDownlaod = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'POST',
      responseType: 'blob',
      url: '/api/app/provider-auth/generate-qr-and-downlaod',
    },
    { apiName: this.apiName,...config });
  

  registerByInput = (input: ProviderRegisterInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/provider-auth/register',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  resendVerficationEmailByTargetEmail = (targetEmail: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/provider-auth/resend-verfication-email',
      params: { targetEmail },
    },
    { apiName: this.apiName,...config });
  

  sendVerificationEmailByEmailAndPharmacyNameAndTenantName = (email: string, pharmacyName: string, tenantName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/provider-auth/send-verification-email',
      params: { email, pharmacyName, tenantName },
    },
    { apiName: this.apiName,...config });
  

  sendWelcomeEmailByEmailAndProviderIdAndPharmacyNameAndTenantName = (email: string, providerId: string, pharmacyName: string, tenantName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/provider-auth/send-welcome-email/${providerId}`,
      params: { email, pharmacyName, tenantName },
    },
    { apiName: this.apiName,...config });
  

  verifyByInput = (input: VerifyCodeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/provider-auth/verify',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  downloadQrCode = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, any>({
      method: 'POST',
      url: '/api/app/provider-auth/download-qr-code',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
