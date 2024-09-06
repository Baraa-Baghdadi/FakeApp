import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { NotificationProviderService, ProviderNotificationDto } from '@proxy/notifications';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationListenerService {
  reciveNewPatientListener = new BehaviorSubject<boolean>(false);
  unreadNotificationCount = new BehaviorSubject<number>(0);
  unreadNotificationCount$ = this.unreadNotificationCount.asObservable();
  unReadingNotificationList = new BehaviorSubject<{}>({});
  NotificationList = new BehaviorSubject<{}>({});
  constructor(private notificationsService : NotificationProviderService) { }

  increaseCount(){
    this.unreadNotificationCount.next(this.unreadNotificationCount.value + 1); 
  }

  makeAllMsgAsReaded(){
    this.notificationsService.markAllAsRead().subscribe();
    this.unreadNotificationCount.next(0);
  }

  // get all unreaded MSG From BE:
  getUnreadedMsg(){
    this.notificationsService.getCountOfUnreadingMsg().subscribe((data:any) => {
      this.unreadNotificationCount.next(data);
    });
  }

  // Get List Of Msg:
  getMsgList(){
    var notificationFilter = {} as PagedAndSortedResultRequestDto;
    this.notificationsService.getListOfProviderNotificationByInput(notificationFilter)
      .subscribe((data) => {
        this.NotificationList.next(data);     
    });
  }
}
