import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { User } from '../../../models/auth.models';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class SidebarComponent {
  @Input() isExpanded = true;
  @Output() toggleSidebar = new EventEmitter<boolean>();
  currentUser: User | null = null;

  navItems = [
    { 
      icon: 'fas fa-th-large', 
      label: 'Dashboard', 
      route: '/dashboard',
      description: 'Overview and statistics'
    },
    {
      icon: 'fas fa-carrot',
      label: 'Ingredients',
      route: '/ingredients',
      description: 'Manage ingredients'
    },
    {
      icon: 'fa-solid fa-scroll',
      label: 'Recipes',
      route: '/recipes',
      description: 'Manage recipes'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  handleSidebarToggle(): void {
    this.isExpanded = !this.isExpanded;
    this.toggleSidebar.emit(this.isExpanded);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isCurrentRoute(route: string): boolean {
    return this.router.url === route;
  }

  getInitials(email: string | undefined): string {
    if (!email) return 'U';
    const parts = email.split('@')[0].split('.');
    return parts.map(part => part[0]?.toUpperCase() || '').join('');
  }
}
