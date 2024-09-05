import { AuthService, LocalizationService, SessionStateService } from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { getLocaleDirection } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map } from 'rxjs';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.scss'
})
export class WelcomeComponent {
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
    if (this.authService.isAuthenticated) router.navigateByUrl('/');
    this.listenToLanguageChanges();
  }
  logIn(){
    this.authService.navigateToLogin();
  }
  get hasLoggedIn():boolean{
    return this.authService.isAuthenticated;
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
