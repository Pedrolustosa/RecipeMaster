import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
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
  filteredRecipes: Recipe[] = [];
  searchTerm: string = '';
  selectedRecipe: Recipe | null = null;
  private deleteModal: any;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private translate: TranslateService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
    this.deleteModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
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
          this.translate.instant('RECIPES.LIST.MESSAGES.LOAD_ERROR'),
          this.translate.instant('COMMON.ERROR')
        );
        this.spinner.hide();
      }
    });
  }

  filterRecipes(): void {
    if (!this.searchTerm.trim()) {
      this.filteredRecipes = [...this.recipes];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredRecipes = this.recipes.filter(recipe =>
      recipe.recipeName.toLowerCase().includes(searchTermLower)
    );
  }

  onSearch(): void {
    this.filterRecipes();
  }

  confirmDelete(recipe: Recipe): void {
    this.selectedRecipe = recipe;
    this.deleteModal.show();
  }

  deleteRecipe(): void {
    if (!this.selectedRecipe) return;

    this.spinner.show();
    this.recipeService.delete(this.selectedRecipe.id).subscribe({
      next: () => {
        this.toastr.success(
          this.translate.instant('RECIPES.LIST.MESSAGES.DELETE_SUCCESS'),
          this.translate.instant('COMMON.SUCCESS')
        );
        this.loadRecipes();
        this.deleteModal.hide();
        this.selectedRecipe = null;
      },
      error: (error) => {
        console.error('Error deleting recipe:', error);
        this.toastr.error(
          this.translate.instant('RECIPES.LIST.MESSAGES.DELETE_ERROR'),
          this.translate.instant('COMMON.ERROR')
        );
        this.spinner.hide();
      }
    });
  }

  cancelDelete(): void {
    this.selectedRecipe = null;
    this.deleteModal.hide();
  }

  downloadRecipePDF(recipe: Recipe): void {
    const doc = new jsPDF();
    const pageWidth = doc.internal.pageSize.width;
    const pageHeight = doc.internal.pageSize.height;
    let yPosition = 20;
    const lineHeight = 7;
    const margin = 20;

    doc.setFillColor(252, 248, 244);
    doc.rect(0, 0, pageWidth, pageHeight, 'F');

    doc.setDrawColor(205, 164, 133);
    doc.setLineWidth(0.5);
    doc.rect(10, 10, pageWidth - 20, pageHeight - 20, 'S');

    doc.setFillColor(205, 164, 133);
    doc.rect(0, 0, pageWidth, 40, 'F');
    
    doc.setTextColor(255, 255, 255);
    doc.setFontSize(24);
    doc.setFont('helvetica', 'bold');
    doc.text(recipe.recipeName, pageWidth / 2, 25, { align: 'center' });

    doc.setFontSize(12);
    doc.setFont('helvetica', 'italic');
    doc.text('RecipeMaster', pageWidth / 2, 35, { align: 'center' });

    yPosition = 60;

    doc.setTextColor(139, 69, 19);
    doc.setFontSize(16);
    doc.setFont('helvetica', 'bold');
    doc.text('* ' + this.translate.instant('RECIPES.LIST.DETAILS.PRODUCTION_DETAILS'), margin, yPosition);
    yPosition += lineHeight * 2;

    doc.setFillColor(255, 253, 250);
    doc.setDrawColor(205, 164, 133);
    doc.roundedRect(margin - 5, yPosition - 5, pageWidth - (2 * margin) + 10, 40, 3, 3, 'FD');
    
    doc.setFontSize(12);
    doc.setFont('helvetica', 'normal');
    const details = [
      `* ${this.translate.instant('RECIPES.LIST.DETAILS.QUANTITY')}: ${recipe.quantity}`,
      `$ ${this.translate.instant('RECIPES.LIST.DETAILS.UNIT_COST')}: ${recipe.unitCost.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`,
      `* ${this.translate.instant('RECIPES.LIST.DETAILS.QUANTITY_PER_PRODUCTION')}: ${recipe.quantityPerProduction}`,
      `$ ${this.translate.instant('RECIPES.LIST.DETAILS.PRODUCTION_COST')}: ${recipe.productionCost.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`
    ];

    details.forEach(detail => {
      doc.text(detail, margin, yPosition);
      yPosition += lineHeight;
    });
    yPosition += lineHeight * 2;

    if (recipe.ingredients && recipe.ingredients.length > 0) {
      doc.setTextColor(139, 69, 19);
      doc.setFontSize(16);
      doc.setFont('helvetica', 'bold');
      doc.text('* ' + this.translate.instant('RECIPES.LIST.DETAILS.INGREDIENTS'), margin, yPosition);
      yPosition += lineHeight * 2;

      const ingredientBoxHeight = recipe.ingredients.length * lineHeight + 10;
      doc.setFillColor(255, 253, 250);
      doc.roundedRect(margin - 5, yPosition - 5, pageWidth - (2 * margin) + 10, ingredientBoxHeight, 3, 3, 'FD');

      doc.setTextColor(0);
      doc.setFontSize(12);
      doc.setFont('helvetica', 'normal');
      recipe.ingredients.forEach(ingredient => {
        doc.text(`- ${ingredient.ingredientName}: ${ingredient.quantity}`, margin, yPosition);
        yPosition += lineHeight;
      });
    }

    doc.setFontSize(8);
    doc.setTextColor(128, 128, 128);
    doc.text('Gerado por RecipeMaster', pageWidth / 2, pageHeight - 15, { align: 'center' });
    const currentDate = new Date().toLocaleDateString('pt-BR');
    doc.text(currentDate, pageWidth - margin, pageHeight - 15, { align: 'right' });

    const fileName = `${recipe.recipeName.replace(/\s+/g, '_')}.pdf`;
    doc.save(fileName);
    
    this.toastr.success(
      this.translate.instant('RECIPES.LIST.MESSAGES.PDF_SUCCESS'),
      this.translate.instant('COMMON.SUCCESS')
    );
  }
}
