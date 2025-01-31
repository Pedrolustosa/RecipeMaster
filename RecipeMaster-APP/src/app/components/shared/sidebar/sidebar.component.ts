import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../../services/auth.service';
import { User } from '../../../models/auth.models';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() isExpanded = true;
  @Output() toggleSidebar = new EventEmitter<boolean>();
  currentUser: User | null = null;

  navItems = [
    {
      icon: 'fas fa-chart-line',
      route: '/dashboard',
      translationKey: 'SIDEBAR.NAV.DASHBOARD'
    },
    {
      icon: 'fas fa-book',
      route: '/recipes',
      translationKey: 'SIDEBAR.NAV.RECIPES'
    },
    {
      icon: 'fas fa-carrot',
      route: '/ingredients',
      translationKey: 'SIDEBAR.NAV.INGREDIENTS'
    }
  ];

  constructor(
    private router: Router,
    private authService: AuthService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
    
    this.translate.addLangs(['pt', 'en']);
    this.translate.setDefaultLang('pt');
    const browserLang = this.translate.getBrowserLang();
    this.translate.use(browserLang?.match(/pt|en/) ? browserLang : 'pt');
  }

  handleSidebarToggle(): void {
    this.isExpanded = !this.isExpanded;
    this.toggleSidebar.emit(this.isExpanded);
  }

  getInitials(email: string | undefined): string {
    if (!email) return '';
    return email
      .split('@')[0]
      .split('.')
      .map(part => part[0]?.toUpperCase() || '')
      .join('');
  }

  isCurrentRoute(route: string): boolean {
    return this.router.url.startsWith(route);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
