import { AuthService, ConfigStateService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProviderAuthService } from '@proxy/provider-auth';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService , 
    private router:Router,private providerService:ProviderAuthService) {
    if (!this.authService.isAuthenticated) this.router.navigateByUrl("/welcome");
  }

  login() {
    this.authService.navigateToLogin();
  }


  // Download QR:
  downloadQr(){
    this.providerService.generateQrAndDownlaod().subscribe(res => {
      const blob = new Blob([res],{type:"application/pdf"});
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = "QR Code.pdf";
      link.click();
    })
  }
}
