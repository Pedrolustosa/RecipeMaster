import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Ingredient } from '../../../models/ingredient.model';
import { IngredientService } from '../../../services/ingredient.service';

@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, NgxSpinnerModule]
})
export class IngredientListComponent implements OnInit {
  ingredients: Ingredient[] = [];
  searchTerm = '';
  sortColumn = 'name';
  sortDirection = 'asc';
  currentPage = 1;
  pageSize = 10;
  loading = false;

  constructor(
    private ingredientService: IngredientService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  private loadIngredients(): void {
    this.loading = true;
    this.ingredientService.getAll().subscribe({
      next: (ingredients: Ingredient[]) => {
        this.ingredients = ingredients;
        this.loading = false;
      },
      error: (error: Error) => {
        this.toastr.error('Failed to load ingredients', 'Error');
        console.error('Error loading ingredients:', error);
        this.loading = false;
      }
    });
  }

  deleteIngredient(id: string): void {
    if (confirm('Are you sure you want to delete this ingredient?')) {
      this.loading = true;
      this.ingredientService.delete(id).subscribe({
        next: () => {
          this.toastr.success('Ingredient deleted successfully', 'Success');
          this.loading = false;
        },
        error: (error: Error) => {
          this.toastr.error('Failed to delete ingredient', 'Error');
          console.error('Error deleting ingredient:', error);
          this.loading = false;
        }
      });
    }
  }

  sortIngredients(column: keyof Ingredient): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.ingredients.sort((a: Ingredient, b: Ingredient) => {
      const valueA = a[column];
      const valueB = b[column];
      const direction = this.sortDirection === 'asc' ? 1 : -1;

      if (typeof valueA === 'string') {
        return valueA.localeCompare(valueB as string) * direction;
      }
      return ((valueA as number) - (valueB as number)) * direction;
    });
  }

  get filteredIngredients(): Ingredient[] {
    return this.ingredients.filter(ingredient =>
      Object.values(ingredient).some(value =>
        value.toString().toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    );
  }

  get paginatedIngredients(): Ingredient[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.filteredIngredients.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredIngredients.length / this.pageSize);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
}
