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
          this.translate.instant('COMMON.ERROR')
        );
        this.loading = false;
      }
    });
  }

  get filteredIngredients(): Ingredient[] {
    return this.ingredients
      .filter(ingredient =>
        Object.values(ingredient).some(value =>
          value?.toString().toLowerCase().includes(this.searchTerm.toLowerCase())
        )
      )
      .sort((a, b) => {
        const aValue = this.getValueForSort(a, this.sortColumn);
        const bValue = this.getValueForSort(b, this.sortColumn);
        
        if (aValue === bValue) return 0;
        
        const comparison = aValue < bValue ? -1 : 1;
        return this.sortDirection === 'asc' ? comparison : -comparison;
      });
  }

  private getValueForSort(ingredient: Ingredient, column: string): any {
    const value = ingredient[column as keyof Ingredient];
    return value === null || value === undefined ? '' : value;
  }

  sortIngredients(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
  }

  confirmDelete(id: string): void {
    this.ingredientToDelete = id;
    this.deleteModal.show();
  }

  deleteIngredient(): void {
    if (!this.ingredientToDelete) return;

    this.loading = true;
    this.ingredientService.delete(this.ingredientToDelete).subscribe({
      next: () => {
        this.toastr.success(
          this.translate.instant('INGREDIENTS.LIST.MESSAGES.DELETE_SUCCESS'),
          this.translate.instant('COMMON.SUCCESS')
        );
        this.loadIngredients();
        this.deleteModal.hide();
      },
      error: (error: Error) => {
        this.toastr.error(
          this.translate.instant('INGREDIENTS.LIST.MESSAGES.DELETE_ERROR'),
          this.translate.instant('COMMON.ERROR')
        );
        this.loading = false;
      }
    });
  }

  cancelDelete(): void {
    this.ingredientToDelete = null;
    this.deleteModal.hide();
  }

  getStockLevelClass(ingredient: Ingredient): string {
    const stockPercentage = (ingredient.stockQuantity / ingredient.minimumStockLevel) * 100;
    if (stockPercentage <= 30) {
      return 'bg-danger';
    } else if (stockPercentage <= 65) {
      return 'bg-warning';
    }
    return 'bg-success';
  }

  getStockPercentage(ingredient: Ingredient): number {
    return Math.min((ingredient.stockQuantity / ingredient.minimumStockLevel) * 100, 100);
  }

  selectIngredient(ingredient: Ingredient): void {
    this.selectedIngredient = ingredient;
  }
}