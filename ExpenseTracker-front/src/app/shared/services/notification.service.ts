import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';  

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private toastr: ToastrService) { } 

  showSuccess(message: string): void {
    this.toastr.success(message, 'Succès');  
  }

  showError(message: string): void {
    this.toastr.error(message, 'Erreur');  
  }

  showInfo(message: string): void {
    this.toastr.info(message, 'Information'); 
  }

  showWarning(message: string): void {
    this.toastr.warning(message, 'Attention');  
  }
}
