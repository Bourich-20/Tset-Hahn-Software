import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { SharedModule } from '../../shared/shared.module';
import { SidebarComponent } from '../../shared/sidebar/sidebar.component';

@NgModule({
  declarations: [
    HomeComponent 
  ],
  imports: [
    CommonModule,
    RouterModule,   
    SharedModule  ,
    SidebarComponent   ]
})
export class HomeModule { }
