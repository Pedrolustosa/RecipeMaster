import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Ingredient } from '../../../models/ingredient.model';
import { IngredientService } from '../../../services/ingredient.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
declare var bootstrap: any;

@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, NgxSpinnerModule, TranslateModule]
})
export class IngredientListComponent implements OnInit {
  ingredients: Ingredient[] = [];
  searchTerm = '';
  sortColumn = 'name';
  sortDirection = 'asc';
  currentPage = 1;
  pageSize = 10;
  loading = false;
  private deleteModal: any;
  private ingredientToDelete: string | null = null;
  selectedIngredient: Ingredient | null = null;

  constructor(
    private ingredientService: IngredientService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.loadIngredients();
    this.deleteModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
  }

  private loadIngredients(): void {
    this.loading = true;
    this.ingredientService.getAll().subscribe({
      next: (ingredients: Ingredient[]) => {
        this.ingredients = ingredients;
        this.loading = false;
      },
      error: (error: Error) => {
        this.toastr.error(
          this.translate.instant('INGREDIENTS.LIST.MESSAGES.LOAD_ERROR'),
          this.translate.instant('INGREDIENTS.LIST.MESSAGES.ERROR')
        );
        console.error('Error loading ingredients:', error);
        this.loading = false;
      }
    });
  }

  selectIngredient(ingredient: Ingredient): void {
    this.selectedIngredient = ingredient;
  }

  deleteIngredient(): void {
    if (this.selectedIngredient) {
      this.ingredientService.delete(this.selectedIngredient.id).subscribe({
        next: () => {
          const modal = document.getElementById('deleteConfirmationModal');
          if (modal) {
            const modalInstance = bootstrap.Modal.getInstance(modal);
            if (modalInstance) {
              modalInstance.hide();
            }
          }
          
          this.toastr.success(
            this.translate.instant('INGREDIENTS.LIST.MESSAGES.DELETE_SUCCESS'),
            this.translate.instant('INGREDIENTS.LIST.MESSAGES.SUCCESS')
          );
          
          this.loadIngredients();
        },
        error: (error) => {
          console.error('Error deleting ingredient:', error);
          this.toastr.error(
            this.translate.instant('INGREDIENTS.LIST.MESSAGES.DELETE_ERROR'),
            this.translate.instant('INGREDIENTS.LIST.MESSAGES.ERROR')
          );
        }
      });
    }
  }

  confirmDelete(): void {
    if (!this.ingredientToDelete) return;

    this.loading = true;
    this.ingredientService.delete(this.ingredientToDelete).subscribe({
      next: () => {
        this.toastr.success('Ingredient deleted successfully', 'Success');
        this.loadIngredients();
        this.deleteModal.hide();
        this.ingredientToDelete = null;
        this.selectedIngredient = null;
      },
      error: (error: Error) => {
        this.toastr.error('Failed to delete ingredient', 'Error');
        console.error('Error deleting ingredient:', error);
        this.loading = false;
      }
    });
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

  getStockLevelClass(ingredient: Ingredient): string {
    const stockPercentage = (ingredient.stockQuantity / ingredient.minimumStockLevel) * 100;
    if (stockPercentage <= 30) {
      return 'bg-danger';
    } else if (stockPercentage <= 65) {
      return 'bg-warning';
    } else {
      return 'bg-success';
    }
  }

  getStockPercentage(ingredient: Ingredient): number {
    return Math.min((ingredient.stockQuantity / ingredient.minimumStockLevel) * 100, 100);
  }
}