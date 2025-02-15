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
    const margin = 20;
    const lineHeight = 7;
    let yPosition = 20;
  
    const colors = {
      background: { r: 252, g: 248, b: 244 },
      border: { r: 205, g: 164, b: 133 },
      header: { r: 205, g: 164, b: 133 },
      boxBackground: { r: 255, g: 253, b: 250 },
      textPrimary: { r: 139, g: 69, b: 19 },
      footerText: { r: 128, g: 128, b: 128 },
    };
  
    doc.setFillColor(colors.background.r, colors.background.g, colors.background.b);
    doc.rect(0, 0, pageWidth, pageHeight, 'F');
  
    doc.setDrawColor(colors.border.r, colors.border.g, colors.border.b);
    doc.setLineWidth(0.5);
    doc.rect(10, 10, pageWidth - 20, pageHeight - 20, 'S');
  
    doc.setFillColor(colors.header.r, colors.header.g, colors.header.b);
    doc.rect(0, 0, pageWidth, 40, 'F');
  
    doc.setTextColor(255, 255, 255);
    doc.setFont('helvetica', 'bold');
    doc.setFontSize(24);
    doc.text(recipe.recipeName, pageWidth / 2, 25, { align: 'center' });
  
    doc.setFont('helvetica', 'italic');
    doc.setFontSize(12);
    doc.text('RecipeMaster', pageWidth / 2, 35, { align: 'center' });
  
    yPosition = 60;
    doc.setTextColor(colors.textPrimary.r, colors.textPrimary.g, colors.textPrimary.b);
    doc.setFont('helvetica', 'bold');
    doc.setFontSize(16);
    const productionTitle = `* ${this.translate.instant('RECIPES.LIST.DETAILS.PRODUCTION_DETAILS')}`;
    doc.text(productionTitle, margin, yPosition);
    yPosition += lineHeight * 2;
  
    doc.setFillColor(colors.boxBackground.r, colors.boxBackground.g, colors.boxBackground.b);
    doc.setDrawColor(colors.border.r, colors.border.g, colors.border.b);
    const productionBoxHeight = 40;
    doc.roundedRect(margin - 5, yPosition - 5, pageWidth - (2 * margin) + 10, productionBoxHeight, 3, 3, 'FD');
  
    doc.setFont('helvetica', 'normal');
    doc.setFontSize(12);
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
      doc.setTextColor(colors.textPrimary.r, colors.textPrimary.g, colors.textPrimary.b);
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(16);
      const ingredientsTitle = `* ${this.translate.instant('RECIPES.LIST.DETAILS.INGREDIENTS')}`;
      doc.text(ingredientsTitle, margin, yPosition);
      yPosition += lineHeight * 2;
  
      const ingredientBoxHeight = recipe.ingredients.length * lineHeight + 10;
      doc.setFillColor(colors.boxBackground.r, colors.boxBackground.g, colors.boxBackground.b);
      doc.roundedRect(margin - 5, yPosition - 5, pageWidth - (2 * margin) + 10, ingredientBoxHeight, 3, 3, 'FD');
  
      doc.setTextColor(0, 0, 0);
      doc.setFont('helvetica', 'normal');
      doc.setFontSize(12);
      recipe.ingredients.forEach(ingredient => {
        doc.text(`- ${ingredient.ingredientName}: ${ingredient.quantity}`, margin, yPosition);
        yPosition += lineHeight;
      });
    }
  
    doc.setFontSize(8);
    doc.setTextColor(colors.footerText.r, colors.footerText.g, colors.footerText.b);
    const footerText = 'RecipeMaster';
    doc.text(footerText, pageWidth / 2, pageHeight - 15, { align: 'center' });
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
