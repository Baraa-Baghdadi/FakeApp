import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { LoaderService } from '../services/loader/loader.service';
import { delay, finalize, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private spinner : LoaderService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.method === "POST") {
      this.spinner.busy();
      return next.handle(request).pipe(
        delay(500),
        finalize(()=>this.spinner.idle())
      ); 
    }
    else{
      return next.handle(request);
    }
  }
}