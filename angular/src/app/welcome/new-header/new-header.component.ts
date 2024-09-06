import { AuthService, getLocaleDirection, LocalizationService, SessionStateService } from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map } from 'rxjs';

@Component({
  selector: 'app-new-header',
  templateUrl: './new-header.component.html',
  styleUrl: './new-header.component.scss'
})
export class NewHeaderComponent {
  availableLangs = [{value:"en",name:"English"},{value:"ar",name:"العربية"}];
  selectedLang = this.sessionState.getLanguage();
  labelOfSelectedLang = this.selectedLang === "en" ? "English" : "العربية" ;
  showLangList = false;

  private dir = new BehaviorSubject<LocaleDirection>('ltr');
  dir$ = this.dir.asObservable();

  constructor(private authService:AuthService,
    private router:Router,
    private sessionState:SessionStateService,
    private localizationService : LocalizationService) { 
      this.listenToLanguageChanges();
     }

    selectLang(lang:any){
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
}
