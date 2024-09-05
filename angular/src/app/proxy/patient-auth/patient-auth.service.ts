import type { LoginDto, PatientAuthDto, PatientLoginDto, RefreshTokenInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { Response } from '../api-response/models';

@Injectable({
  providedIn: 'root',
})
export class PatientAuthService {
  apiName = 'Default';
  

  authenticateUserByInput = (input: PatientAuthDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<LoginDto>>({
      method: 'POST',
      url: '/api/app/patient-auth/authenticate-user',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getRefreshTokenByInput = (input: RefreshTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<LoginDto>>({
      method: 'POST',
      url: '/api/app/patient-auth/get-refresh-token',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  logInByInput = (input: PatientLoginDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'POST',
      url: '/api/app/patient-auth/log-in',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  resendOtpByInput = (input: PatientLoginDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'POST',
      url: '/api/app/patient-auth/resend-otp',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  _checkCodeByDto = (dto: PatientAuthDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Response<boolean>>({
      method: 'POST',
      url: '/api/app/patient-auth/_check-code',
      body: dto,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
