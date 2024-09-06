import { AuthService, ReplaceableComponentsService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalR/signal-r.service';
import { eThemeSharedComponents } from '@abp/ng.core';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';
import { NewHeaderComponent } from './welcome/new-header/new-header.component';


@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
    <abp-internet-status></abp-internet-status>
  `,
})
export class AppComponent implements OnInit {

  constructor(private authService:AuthService,private signalR:SignalRService,
    private replaceableComponent: ReplaceableComponentsService,
  ){
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated){
      this.signalR.connect();
    }
    this.replaceableComponent.add({
      component: NewHeaderComponent,
      key: eThemeLeptonXComponents.Languages,
    });
  }
}
