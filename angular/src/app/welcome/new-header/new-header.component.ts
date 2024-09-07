import { AuthService, ConfigStateService, getLocaleDirection, LocalizationService, SessionStateService } from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { NotificationListenerService } from 'src/app/services/notification/notification-listener.service';

@Component({
  selector: 'app-new-header',
  templateUrl: './new-header.component.html',
  styleUrl: './new-header.component.scss'
})
export class NewHeaderComponent implements OnInit {
  availableLangs = [{value:"en",name:"English"},{value:"ar",name:"العربية"}];
  selectedLang = this.sessionState.getLanguage();
  labelOfSelectedLang = this.selectedLang === "en" ? "English" : "العربية" ;
  showLangList = false;

  private dir = new BehaviorSubject<LocaleDirection>('ltr');
  dir$ = this.dir.asObservable();

  allMsgs : any;
  // For pagination:
  isLoading=false;
  currentPage=1;
  itemsPerPage=10;
  toggleLoading = ()=>this.isLoading=!this.isLoading;

    // it will be called when this component gets initialized.
    loadData= ()=>{
      this.toggleLoading();
      this.getMsgList(this.currentPage,this.itemsPerPage);
    }
    
    // this method will be called on scrolling the page
    appendData= ()=>{
     this.toggleLoading();
     this.getMsgList(this.currentPage,this.itemsPerPage);
   }
 
    onScroll= ()=>{
     this.currentPage++;     
     this.appendData();
    }
 


  ngOnInit(): void {
    const tenantId = this.config.getOne("currentUser").tenantId;
    if (tenantId) {  
      this.getUnreadedMsg();
      this.loadData();
    }
  }

  constructor(
    private config: ConfigStateService,
    private sessionState:SessionStateService,
    public localizationService : LocalizationService,
    public notificationListener : NotificationListenerService) { 
      this.listenToLanguageChanges();
    }
    
    // For Select language:
   selectLang(lang:any){
      this.notificationListener.makeNotificationListEmpty();
      this.sessionState.setLanguage(lang);
      this.selectedLang = this.sessionState.getLanguage();
      this.labelOfSelectedLang = this.selectedLang === "en" ? "English" : "العربية" ;
      this.showLangList = false;  
    }
  
    private listenToLanguageChanges(){
      this.localizationService.currentLang$.pipe(map(locale => getLocaleDirection(locale))).subscribe(dir => {
        this.dir.next(dir);
        this.setBodyDir(dir);
      })
    }
  
    private setBodyDir(dir : LocaleDirection){
      document.body.dir = dir;
      document.dir = dir;
    }

    // For Notifications:
    getUnreadedMsg(){
      this.notificationListener.getUnreadedMsg();         
    }

    makeAllMsgAsReaded(){
      this.notificationListener.makeAllMsgAsReaded();
    }

    getMsgList(page=1,itemsPerPage=10){
      this.notificationListener.getMsgList(page,itemsPerPage);
    }
}
