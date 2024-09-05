
export interface AttachmentCreateDto {
  blop?: string;
  fileType?: string;
  fileName?: string;
  fileSize: number;
}

export interface AttachmentDto {
  id?: string;
  blopName?: string;
  fileType?: string;
  fileName?: string;
  fileSize: number;
}
