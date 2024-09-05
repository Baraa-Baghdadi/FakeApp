
export interface Response<T> {
  status: boolean;
  code: number;
  message?: string;
  data: ResponseObject<T>;
}

export interface ResponseObject<T> {
  result: T;
}
