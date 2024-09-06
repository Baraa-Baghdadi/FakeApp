import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { NotificationListenerService } from '../notification/notification-listener.service';
import { ToastComponent, ToasterService } from '@abp/ng.theme.shared';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor(private NotificationListener : NotificationListenerService
    , private toaster : ToasterService,private OAuthService: OAuthService
  ) { }

  // Connect To SignalR:
  connect(){
    const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(environment.apis.default.url + '/notify',{accessTokenFactory  :  () => this.OAuthService.getAccessToken()})
    .build();
    connection.start().then(function () {
      console.log('SignalR Connected! & connectionId:',connection.connectionId);
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("PatientAddedYouMsg", () => {
      // Send new event for update data in table:
      this.NotificationListener.reciveNewPatientListener.next(true);
      // update notification list:
      setTimeout(() => {
        this.NotificationListener.getMsgList();
      }, 1000);
      // increase count of unread MSG:
      this.NotificationListener.increaseCount();
      // show toaster:
      this.toaster.info("::newPatientAddedYou","",{life: 5000,closable:false});     
    });
  }
}
