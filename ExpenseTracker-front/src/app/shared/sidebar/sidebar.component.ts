import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  imports: [
    CommonModule, 
    RouterModule  
  ],
  standalone: true 
})
export class SidebarComponent {
  isOpened = false; 
  isSidebarCollapsed = false; 
  activeIndex = 0; 
  constructor(private router: Router) {} 
  navItems = [
    { name: 'Add Expense', route: '/home/add-expense', icon: '➕' },
    { name: 'View Expenses', route: '/home/view-expenses', icon: '📄' },
    { name: 'Budget Progress', route: '/home/budget-progress', icon: '📊' },
    { name: 'Settings', route: '/home/settings', icon: '⚙️' },
  ];

  toggleSidebar() {
    this.isOpened = !this.isOpened;
  }

  toggleDesktopSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }


  handleItemClick(index: number) {
    this.activeIndex = index;

    const route = this.navItems[index].route;
    console.log('Navigating to:', route);
    this.router.navigate([route]);
  }
}
