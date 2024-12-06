import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class RegisterComponent {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onRegister() {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = "Passwords don't match.";
      return;
    }

    const user = {
      email: this.email,
      firstName: this.firstName,
      lastName: this.lastName,
      password: this.password,
    };

    this.authService.register(user).subscribe({
      next: () => {
        this.snackBar.open('Account created successfully!', 'Close', {
          duration: 3000, 
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.errorMessage =
          err.error?.message || 'Registration failed. Please try again.';
      },
    });
  }
}
