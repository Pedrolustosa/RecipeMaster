import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import jsPDF from 'jspdf';

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
  loading = false;
  searchTerm: string = '';
  filteredRecipes: Recipe[] = [];
  selectedRecipe: Recipe | null = null;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    this.loadRecipes();
  }

  loadRecipes(): void {
    this.loading = true;
    this.recipeService.getAll().subscribe({
      next: (recipes) => {
        this.recipes = recipes;
        this.filterRecipes();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading recipes:', error);
        this.toastr.error(
          this.translate.instant('RECIPES.MESSAGES.LOAD_ERROR'),
          this.translate.instant('RECIPES.MESSAGES.ERROR')
        );
        this.loading = false;
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
      recipe.name.toLowerCase().includes(searchTermLower) ||
      recipe.description.toLowerCase().includes(searchTermLower)
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
      this.loading = true;
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
          this.loading = false;
        },
        error: (error) => {
          console.error('Error deleting recipe:', error);
          this.toastr.error(
            this.translate.instant('RECIPES.MESSAGES.DELETE_ERROR'),
            this.translate.instant('RECIPES.MESSAGES.ERROR')
          );
          this.loading = false;
        }
      });
    }
  }

  downloadRecipePDF(recipe: Recipe): void {
    this.recipeService.getById(recipe.id).subscribe({
      next: (fullRecipe) => {
        const pdf = new jsPDF();
        const pageWidth = pdf.internal.pageSize.width;
        const pageHeight = pdf.internal.pageSize.height;
        const margin = 20;
        let yPos = margin;

        pdf.setFillColor(251, 247, 238);
        pdf.rect(0, 0, pageWidth, pageHeight, 'F');

        pdf.setDrawColor(139, 69, 19);
        pdf.setLineWidth(2);
        pdf.rect(margin/2, margin/2, pageWidth - margin, pageHeight - margin, 'S');
        pdf.setLineWidth(0.5);
        pdf.rect(margin, margin, pageWidth - margin*2, pageHeight - margin*2, 'S');

        const headerY = margin + 15;
        pdf.setDrawColor(139, 69, 19);
        pdf.setLineWidth(1);
        pdf.line(margin, headerY, pageWidth - margin, headerY);
        yPos = headerY + 15;

        pdf.setFont('times', 'bold');
        pdf.setFontSize(28);
        pdf.setTextColor(101, 67, 33);
        const titleWidth = pdf.getStringUnitWidth(fullRecipe.name) * 28 / pdf.internal.scaleFactor;
        const titleX = (pageWidth - titleWidth) / 2;
        pdf.text(fullRecipe.name, titleX, yPos);
        yPos += 20;

        const contentX = margin + 10;
        const contentWidth = pageWidth - (margin + 10) * 2;

        pdf.setFont('times', 'bold');
        pdf.setFontSize(16);
        pdf.text('Description', contentX, yPos);
        yPos += 10;

        pdf.setFont('times', 'normal');
        pdf.setFontSize(12);
        pdf.setTextColor(0);
        const descriptionLines = pdf.splitTextToSize(fullRecipe.description, contentWidth);
        pdf.text(descriptionLines, contentX, yPos);
        yPos += descriptionLines.length * 7 + 15;

        pdf.setFont('times', 'bold');
        pdf.setFontSize(16);
        pdf.setTextColor(101, 67, 33);
        pdf.text('Recipe Details', contentX, yPos);
        yPos += 10;

        pdf.setFont('times', 'normal');
        pdf.setFontSize(12);
        pdf.setTextColor(0);
        pdf.text(`Preparation Time: ${fullRecipe.preparationTime} minutes`, contentX + 10, yPos);
        yPos += 8;
        pdf.text(`Cooking Time: ${fullRecipe.cookingTime} minutes`, contentX + 10, yPos);
        yPos += 8;
        pdf.text(`Servings: ${fullRecipe.servings}`, contentX + 10, yPos);
        yPos += 8;
        pdf.text(`Difficulty: ${fullRecipe.difficulty}`, contentX + 10, yPos);
        yPos += 15;

        pdf.setFont('times', 'bold');
        pdf.setFontSize(16);
        pdf.setTextColor(101, 67, 33);
        pdf.text('Ingredients', contentX, yPos);
        yPos += 10;

        pdf.setDrawColor(139, 69, 19);
        pdf.setLineWidth(0.5);
        pdf.line(contentX, yPos - 5, contentX + 50, yPos - 5);

        pdf.setFont('times', 'normal');
        pdf.setFontSize(12);
        pdf.setTextColor(0);

        fullRecipe.ingredients.forEach(ingredient => {
          pdf.setFont('zapfdingbats');
          pdf.text('•', contentX, yPos);
          
          pdf.setFont('times', 'normal');
          const ingredientText = `${ingredient.ingredientName}: ${ingredient.quantity}`;
          pdf.text(ingredientText, contentX + 10, yPos);
          yPos += 8;
        });

        yPos += 10;

        if (fullRecipe.instructions) {
          pdf.setFont('times', 'bold');
          pdf.setFontSize(16);
          pdf.setTextColor(101, 67, 33);
          pdf.text('Instructions', contentX, yPos);
          yPos += 10;

          pdf.setFont('times', 'normal');
          pdf.setFontSize(12);
          pdf.setTextColor(0);
          const instructionsLines = pdf.splitTextToSize(fullRecipe.instructions, contentWidth);
          pdf.text(instructionsLines, contentX, yPos);
          yPos += instructionsLines.length * 7 + 15;
        }

        const boxHeight = 40;
        const boxY = yPos;
        pdf.setDrawColor(139, 69, 19);
        pdf.setLineWidth(0.5);
        pdf.rect(contentX, boxY, contentWidth, boxHeight, 'S');

        pdf.setFont('times', 'bold');
        pdf.setTextColor(101, 67, 33);
        pdf.text('Recipe Summary', contentX + 5, boxY + 15);
        
        pdf.setFont('times', 'normal');
        pdf.setTextColor(0);
        pdf.text(`Total Ingredients: ${fullRecipe.ingredients.length}`, contentX + 5, boxY + 25);
        pdf.text(`Estimated Cost: $${fullRecipe.totalCost.toFixed(2)}`, contentX + 5, boxY + 35);

        const footerY = pageHeight - margin - 10;
        pdf.setFont('times', 'italic');
        pdf.setFontSize(10);
        pdf.setTextColor(128);
        
        pdf.setDrawColor(139, 69, 19);
        pdf.setLineWidth(0.5);
        pdf.line(margin, footerY - 15, pageWidth - margin, footerY - 15);

        pdf.text('Generated by RecipeMaster', margin, footerY);
        const dateText = new Date().toLocaleDateString();
        const dateWidth = pdf.getStringUnitWidth(dateText) * 10 / pdf.internal.scaleFactor;
        pdf.text(dateText, pageWidth - margin - dateWidth, footerY);

        const fileName = `${fullRecipe.name.toLowerCase().replace(/\s+/g, '-')}-recipe.pdf`;
        pdf.save(fileName);
      },
      error: (error) => {
        this.toastr.error('Failed to generate PDF', 'Error');
        console.error('Error generating PDF:', error);
      }
    });
  }
}
