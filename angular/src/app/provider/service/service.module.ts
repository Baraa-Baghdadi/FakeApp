import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServiceRoutingModule } from './service-routing.module';
import { ServicesComponent } from './services/services.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThumbnailComponent } from '../shared/thumbnail/thumbnail.component';
@NgModule({
  declarations: [
    ServicesComponent
  ],
  imports: [
    CommonModule,
    ServiceRoutingModule,
    SharedModule,
    ThumbnailComponent
  ]
})
export class ServiceModule { }
