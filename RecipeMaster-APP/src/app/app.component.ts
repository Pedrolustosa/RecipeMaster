import { Component } from '@angular/core';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavbarComponent, SidebarComponent, RouterModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isSidebarCollapsed = false; // Estado inicial da Sidebar
  username = 'John Doe'; // Simulação do nome do usuário logado

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed; // Alterna entre colapsado/expandido
  }
}
