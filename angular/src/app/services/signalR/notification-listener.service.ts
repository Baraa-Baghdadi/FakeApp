import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationListenerService {
  reciveNewPatientListener = new BehaviorSubject<boolean>(false);
  constructor() { }
}
