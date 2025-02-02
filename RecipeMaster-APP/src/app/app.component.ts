import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SidebarComponent } from './components/shared/sidebar/sidebar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    NgxSpinnerModule,
    SidebarComponent
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isSidebarExpanded = true;

  constructor(
    private router: Router,
    private translate: TranslateService
  ) {
    translate.addLangs(['pt', 'en']);
    translate.setDefaultLang('en');
    const browserLang = translate.getBrowserLang();
    translate.use(browserLang && ['pt', 'en'].includes(browserLang) ? browserLang : 'en');
  }

  onSidebarToggle(isExpanded: boolean): void {
    this.isSidebarExpanded = isExpanded;
  }

  shouldShowSidebar(): boolean {
    return !['/login', '/register'].includes(this.router.url);
  }

  switchLanguage(lang: string): void {
    this.translate.use(lang);
  }
}
