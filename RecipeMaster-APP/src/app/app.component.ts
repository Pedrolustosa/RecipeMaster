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
    // Configurar idiomas disponíveis
    translate.addLangs(['pt', 'en']);
    
    // Definir idioma padrão
    translate.setDefaultLang('pt');
    
    // Obter o idioma do navegador
    const browserLang = translate.getBrowserLang();
    
    // Usar o idioma do navegador se disponível, senão usar o padrão
    translate.use(browserLang?.match(/pt|en/) ? browserLang : 'pt');
  }

  onSidebarToggle(isExpanded: boolean): void {
    this.isSidebarExpanded = isExpanded;
  }

  shouldShowSidebar(): boolean {
    const publicRoutes = ['/login', '/register'];
    return !publicRoutes.some(route => this.router.url.includes(route));
  }
}
