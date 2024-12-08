import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [ FormsModule,CommonModule], 
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    if (this.email && this.password) {
      this.authService.login(this.email, this.password).subscribe(
        (response) => {
          console.log('Login successful', response);
          this.authService.setToken(response.token); 
          this.router.navigate(['/home']); 
        },
        (error) => {
          console.error('Login error', error);
          this.errorMessage = error.error?.message || 'Invalid login credentials.';
        }
      );
    } else {
      this.errorMessage = 'Please enter both email and password.';
    }
  }
  goToRegister() {
    console.log('Navigating to register...');
    this.router.navigate(['/register']);
  }
  
}
