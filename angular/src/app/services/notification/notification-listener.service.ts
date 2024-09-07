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
  NotificationList = new BehaviorSubject<any | null>(null);
  NotificationList$ = this.NotificationList.asObservable();
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
  getMsgList(page=1,itemsPerPage=10){
    var notificationFilter = {} as PagedAndSortedResultRequestDto;
    notificationFilter.maxResultCount = itemsPerPage;
    notificationFilter.skipCount = (page * itemsPerPage) - itemsPerPage;
    this.notificationsService.getListOfProviderNotificationByInput(notificationFilter)
      .subscribe((data:any) => {
        if (this.NotificationList.value != null) {
          var oldValue = this.NotificationList.value.items;
          data.items = [...oldValue,...data.items];
          this.NotificationList.next(data);              
        }
        else{          
          this.NotificationList.next(data);         
        }       
    });
  }

  makeNotificationListEmpty(){
    this.NotificationList.next(null); 
  }
}
