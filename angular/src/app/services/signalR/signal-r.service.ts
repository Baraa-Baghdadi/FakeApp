import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() { }

  // Connect To SignalR:
  connectToSignalR(){
    const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(environment.apis.default.url + '/notify')
    .build();

    connection.start().then(function () {
      console.log('SignalR Connected! & connectionId:',connection.connectionId);
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("BroadcastMessage", () => {
      console.log("BroadcastMessage");     
    });
  }
}
