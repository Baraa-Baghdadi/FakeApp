import { AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalR/signal-r.service';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
    <abp-internet-status></abp-internet-status>
  `,
})
export class AppComponent implements OnInit {

  constructor(private authService:AuthService,private signalR:SignalRService){
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated){
      this.signalR.connectToSignalR();
    }
  }
}
