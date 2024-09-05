import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyPatientComponent } from './my-patient/my-patient.component';

const routes: Routes = [
  {path:"",component:MyPatientComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyPatientRoutingModule { }
