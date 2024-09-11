import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateServiceDto {
  title: string;
  arTitle: string;
  isActive: boolean;
  blop: string;
  fileType: string;
  fileName: string;
  fileSize: number;
  isIconUpdated: boolean;
}

export interface GetServieInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
}

export interface ServiceDto extends EntityDto<string> {
  id?: string;
  title?: string;
  arTitle?: string;
  imageId?: string;
  icon?: string;
  isActive: boolean;
  orginalImage?: string;
}
