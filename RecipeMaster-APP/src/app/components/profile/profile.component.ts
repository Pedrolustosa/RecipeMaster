import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../services/auth.service';
import { User, UpdateUserRequest } from '../../models/auth.models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  currentUser: User | null = null;
  userForm: UpdateUserRequest = {
    email: '',
    userName: '',
    id: '',
    isActive: true
  };
  isEditing: boolean = false;
  isSaving: boolean = false;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUserData();
  }

  loadUserData(): void {
    this.currentUser = this.authService.currentUserValue;
    if (this.currentUser) {
      this.authService.getUserById(this.currentUser.id).subscribe({
        next: (user) => {
          this.currentUser = user;
          this.userForm = {
            email: user.email,
            userName: user.userName,
            id: user.id,
            isActive: user.isActive
          };
        },
        error: (error) => {
          this.toastr.error('PROFILE.ERROR.LOAD_DATA');
          console.error('Error loading user data:', error);
        }
      });
    }
  }

  toggleEdit(): void {
    if (this.isEditing) {
      this.userForm = {
        email: this.currentUser?.email || '',
        userName: this.currentUser?.userName || '',
        id: this.currentUser?.id || '',
        isActive: this.currentUser?.isActive || false
      };
    }
    this.isEditing = !this.isEditing;
  }

  saveProfile(): void {
    if (!this.currentUser) return;

    this.isSaving = true;
    this.authService.updateUser(this.currentUser.id, this.userForm).subscribe({
      next: () => {
        this.isEditing = false;
        this.isSaving = false;
        this.toastr.success('PROFILE.SUCCESS.UPDATE');
        this.loadUserData();
      },
      error: (error) => {
        this.isSaving = false;
        this.toastr.error('PROFILE.ERROR.UPDATE');
        console.error('Error updating profile:', error);
      }
    });
  }

  getInitials(): string {
    if (!this.currentUser?.userName) return '';
    return this.currentUser.userName.charAt(0).toUpperCase();
  }
}
