import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.models';
import jsPDF from 'jspdf';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, NgxSpinnerModule]
})
export class RecipeListComponent implements OnInit {
  recipes: Recipe[] = [];
  loading = false;
  searchTerm = '';
  filteredRecipes: Recipe[] = [];
  selectedRecipe: Recipe | null = null;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadRecipes();
  }

  private loadRecipes(): void {
    this.loading = true;
    this.recipeService.getAll().subscribe({
      next: (recipes) => {
        this.recipes = recipes;
        this.filterRecipes();
        this.loading = false;
      },
      error: (error) => {
        this.toastr.error('Failed to load recipes', 'Error');
        console.error('Error loading recipes:', error);
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

  selectRecipe(recipe: Recipe): void {
    this.selectedRecipe = recipe;
  }

  deleteRecipe(id: string | undefined): void {
    if (!id) return;
    
    this.loading = true;
    this.recipeService.delete(id).subscribe({
      next: () => {
        this.toastr.success('Recipe deleted successfully', 'Success');
        this.loadRecipes();
        this.selectedRecipe = null;
      },
      error: (error) => {
        this.toastr.error('Failed to delete recipe', 'Error');
        console.error('Error deleting recipe:', error);
        this.loading = false;
      }
    });
  }

  downloadRecipePDF(recipe: Recipe): void {
    const pdf = new jsPDF();
    const pageWidth = pdf.internal.pageSize.width;
    const pageHeight = pdf.internal.pageSize.height;
    const margin = 20;
    let yPos = margin;

    // Add parchment background color
    pdf.setFillColor(251, 247, 238); // Warm cream color
    pdf.rect(0, 0, pageWidth, pageHeight, 'F');

    // Add decorative border
    pdf.setDrawColor(139, 69, 19); // Saddle brown
    pdf.setLineWidth(2);
    pdf.rect(margin/2, margin/2, pageWidth - margin, pageHeight - margin, 'S');
    pdf.setLineWidth(0.5);
    pdf.rect(margin, margin, pageWidth - margin*2, pageHeight - margin*2, 'S');

    // Add header decoration
    pdf.setDrawColor(139, 69, 19);
    pdf.setLineWidth(1);
    const headerY = margin + 15;
    pdf.line(margin, headerY, pageWidth - margin, headerY);
    yPos = headerY + 15;

    // Add title with elegant font
    pdf.setFont('times', 'bold');
    pdf.setFontSize(28);
    pdf.setTextColor(101, 67, 33); // Dark brown
    const titleWidth = pdf.getStringUnitWidth(recipe.name) * 28 / pdf.internal.scaleFactor;
    const titleX = (pageWidth - titleWidth) / 2;
    pdf.text(recipe.name, titleX, yPos);
    yPos += 20;

    // Add recipe details
    const contentX = margin + 10;
    const contentWidth = pageWidth - (margin + 10) * 2;

    // Description section
    pdf.setFont('times', 'bold');
    pdf.setFontSize(16);
    pdf.text('Description', contentX, yPos);
    yPos += 10;

    pdf.setFont('times', 'normal');
    pdf.setFontSize(12);
    pdf.setTextColor(0);
    const descriptionLines = pdf.splitTextToSize(recipe.description, contentWidth);
    pdf.text(descriptionLines, contentX, yPos);
    yPos += descriptionLines.length * 7 + 15;

    // Ingredients section
    pdf.setFont('times', 'bold');
    pdf.setFontSize(16);
    pdf.setTextColor(101, 67, 33);
    pdf.text('Ingredients', contentX, yPos);
    yPos += 10;

    // Add decorative line under section title
    pdf.setDrawColor(139, 69, 19);
    pdf.setLineWidth(0.5);
    pdf.line(contentX, yPos - 5, contentX + 50, yPos - 5);

    pdf.setFont('times', 'normal');
    pdf.setFontSize(12);
    pdf.setTextColor(0);

    let totalCost = 0;
    recipe.ingredients.forEach(ingredient => {
      // Add bullet point
      pdf.setFont('zapfdingbats');
      pdf.text('•', contentX, yPos);
      
      // Add ingredient text
      pdf.setFont('times', 'normal');
      const ingredientText = `${ingredient.ingredientName}: ${ingredient.quantity} units`;
      pdf.text(ingredientText, contentX + 10, yPos);
      yPos += 8;
    });

    yPos += 10;

    // Add summary box
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
    pdf.text(`Total Ingredients: ${recipe.ingredients.length}`, contentX + 5, boxY + 25);
    pdf.text(`Estimated Cost: $${totalCost.toFixed(2)}`, contentX + 5, boxY + 35);

    // Add footer
    const footerY = pageHeight - margin - 10;
    pdf.setFont('times', 'italic');
    pdf.setFontSize(10);
    pdf.setTextColor(128);
    
    // Add decorative line above footer
    pdf.setDrawColor(139, 69, 19);
    pdf.setLineWidth(0.5);
    pdf.line(margin, footerY - 15, pageWidth - margin, footerY - 15);

    // Footer text
    pdf.text('Generated by RecipeMaster', margin, footerY);
    const dateText = new Date().toLocaleDateString();
    const dateWidth = pdf.getStringUnitWidth(dateText) * 10 / pdf.internal.scaleFactor;
    pdf.text(dateText, pageWidth - margin - dateWidth, footerY);

    // Save the PDF
    const fileName = `${recipe.name.toLowerCase().replace(/\s+/g, '-')}-recipe.pdf`;
    pdf.save(fileName);
    this.toastr.success('Recipe PDF downloaded successfully', 'Success');
  }
}
