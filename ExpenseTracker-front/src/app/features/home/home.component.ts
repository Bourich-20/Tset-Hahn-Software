import { Component } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { SidebarComponent } from '../../shared/sidebar/sidebar.component'; 
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [SharedModule, SidebarComponent, CommonModule, RouterModule],
})
export class HomeComponent {
  user = {
    firstName: 'SOUFIANE',
    lastName: 'BOURICH',
    email: 'soufianbourich20@gmail.com',
    avatar: '/profilImage.jpeg',
  };
}
