import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { SharedModule } from '../shared/shared.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { WelcomeRoutingModule } from './welcome-routing.module';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NewHeaderComponent } from './new-header/new-header.component';
import { InfiniteScrollModule } from "ngx-infinite-scroll";



@NgModule({
  declarations: [
    WelcomeComponent,
    NewHeaderComponent
  ],
  imports: [
    CommonModule,
    WelcomeRoutingModule,
    ThemeSharedModule,
    SharedModule,
    NgbDropdownModule,
    InfiniteScrollModule
  ]
})
export class WelcomeModule { }
