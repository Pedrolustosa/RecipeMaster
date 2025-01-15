import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './components/shared/sidebar/sidebar.component';
import { Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, NgxSpinnerModule]
})
export class AppComponent {
  isSidebarExpanded = true;

  constructor(private router: Router) {}

  onSidebarToggle(isExpanded: boolean): void {
    this.isSidebarExpanded = isExpanded;
  }

  shouldShowSidebar(): boolean {
    return !this.router.url.includes('/login');
  }
}
