
export interface LoginDto {
  mobileNumber?: string;
  token?: string;
  refreshToken?: string;
}

export interface PatientAuthDto {
  countryCode: string;
  mobileNumber: string;
  code: string;
  deviceToken?: string;
}

export interface PatientLoginDto {
  countryCode?: string;
  mobileNumber?: string;
}

export interface RefreshTokenInput {
  refreshToken?: string;
}
