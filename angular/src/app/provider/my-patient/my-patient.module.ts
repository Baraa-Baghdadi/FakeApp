import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyPatientRoutingModule } from './my-patient-routing.module';
import { MyPatientComponent } from './my-patient/my-patient.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    MyPatientComponent
  ],
  imports: [
    CommonModule,
    MyPatientRoutingModule,
    SharedModule
  ]
})
export class MyPatientModule { }
