import type { ProviderAddressDto, WorkingTimeDto } from '../providers/models';

export interface ProviderRegisterInfoDto {
  password?: string;
  email?: string;
  pharmacyName?: string;
  pharmacyPhone?: string;
  locationInfo: ProviderAddressDto;
  workingTimes: WorkingTimeDto[];
}

export interface VerifyCodeDto {
  token?: string;
  email?: string;
}
