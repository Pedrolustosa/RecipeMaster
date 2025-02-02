import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

declare var bootstrap: any;

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    NgxSpinnerModule,
    RouterModule
  ]
})
export class RecipeListComponent implements OnInit {
  recipes: Recipe[] = [];
  filteredRecipes: Recipe[] = [];
  searchTerm: string = '';
  selectedRecipe: Recipe | null = null;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private translate: TranslateService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  loadRecipes(): void {
    this.spinner.show();
    this.recipeService.getAll().subscribe({
      next: (recipes) => {
        this.recipes = recipes;
        this.filterRecipes();
        this.spinner.hide();
      },
      error: (error) => {
        console.error('Error loading recipes:', error);
        this.toastr.error(
          this.translate.instant('RECIPES.MESSAGES.LOAD_ERROR'),
          this.translate.instant('RECIPES.MESSAGES.ERROR')
        );
        this.spinner.hide();
      }
    });
  }

  filterRecipes(): void {
    if (!this.searchTerm) {
      this.filteredRecipes = this.recipes;
      return;
    }
    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredRecipes = this.recipes.filter(recipe =>
      recipe.recipeName.toLowerCase().includes(searchTermLower) ||
      recipe.productionCost.toString().toLowerCase().includes(searchTermLower)
    );
  }

  onSearch(): void {
    this.filterRecipes();
  }

  setSelectedRecipe(recipe: Recipe): void {
    this.selectedRecipe = recipe;
  }

  deleteRecipe(): void {
    if (this.selectedRecipe) {
      this.spinner.show();
      this.recipeService.delete(this.selectedRecipe.id).subscribe({
        next: () => {
          const modal = document.getElementById('deleteConfirmationModal');
          if (modal) {
            const modalInstance = bootstrap.Modal.getInstance(modal);
            if (modalInstance) {
              modalInstance.hide();
            }
          }
          this.toastr.success(
            this.translate.instant('RECIPES.MESSAGES.DELETE_SUCCESS'),
            this.translate.instant('RECIPES.MESSAGES.SUCCESS')
          );
          this.loadRecipes();
          this.selectedRecipe = null;
          this.spinner.hide();
        },
        error: (error) => {
          console.error('Error deleting recipe:', error);
          this.toastr.error(
            this.translate.instant('RECIPES.MESSAGES.DELETE_ERROR'),
            this.translate.instant('RECIPES.MESSAGES.ERROR')
          );
          this.spinner.hide();
        }
      });
    }
  }

  downloadRecipePDF(recipe: Recipe): void {
    this.spinner.show();
    this.recipeService.getById(recipe.id).subscribe({
      next: (fullRecipe) => {
        this.spinner.hide();
      },
      error: (error) => {
        this.toastr.error('Failed to generate PDF', 'Error');
        console.error('Error generating PDF:', error);
        this.spinner.hide();
      }
    });
  }
}
