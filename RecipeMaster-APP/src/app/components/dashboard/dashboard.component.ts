import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgChartsModule, BaseChartDirective } from 'ng2-charts';
import { ChartData, ChartOptions } from 'chart.js';
import { DashboardService } from '../../services/dashboard.service';
import { AuthService } from '../../services/auth.service';
import { 
  IngredientCostDTO, 
  IngredientUsageDTO
} from '../../models/dashboard.models';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, NgChartsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  totalRecipes: number = 0;
  totalIngredients: number = 0;
  totalRecipeCost: number = 0;
  averageRecipeCost: number = 0;
  topExpensiveIngredients: IngredientCostDTO[] = [];
  mostUsedIngredients: IngredientUsageDTO[] = [];

  // Cost Chart Configuration
  costChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Cost R($)',
      backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF']
    }]
  };

  costChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          callback: (value) => `R$${value}`
        }
      }
    },
    plugins: {
      legend: { 
        display: false 
      },
      title: { 
        display: true, 
        text: 'Top 5 Most Expensive Ingredients',
        padding: 20
      }
    }
  };

  // Usage Chart Configuration
  usageChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      backgroundColor: ['#4BC0C0', '#FFCE56', '#36A2EB', '#FF6384', '#9966FF'],
      borderColor: ['#4BC0C0', '#FFCE56', '#36A2EB', '#FF6384', '#9966FF'],
      borderWidth: 1,
      barThickness: 25,
      borderRadius: 4
    }]
  };

  usageChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
    indexAxis: 'y',
    layout: {
      padding: {
        left: 10,
        right: 25,
        top: 0,
        bottom: 0
      }
    },
    plugins: {
      legend: { 
        display: false
      },
      tooltip: {
        callbacks: {
          label: (context) => `${context.formattedValue} recipes`
        }
      }
    },
    scales: {
      x: {
        beginAtZero: true,
        grid: {
          display: false
        },
        ticks: {
          stepSize: 1
        }
      },
      y: {
        grid: {
          display: false
        }
      }
    }
  };

  constructor(
    private dashboardService: DashboardService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    forkJoin({
      totalRecipes: this.dashboardService.getTotalRecipes(),
      totalIngredients: this.dashboardService.getTotalIngredients(),
      totalRecipeCost: this.dashboardService.getTotalRecipeCost(),
      averageRecipeCost: this.dashboardService.getAverageRecipeCost(),
      topExpensiveIngredients: this.dashboardService.getMostExpensiveIngredients(),
      mostUsedIngredients: this.dashboardService.getMostUsedIngredients()
    }).subscribe({
      next: (data) => {
        this.totalRecipes = data.totalRecipes;
        this.totalIngredients = data.totalIngredients;
        this.totalRecipeCost = data.totalRecipeCost;
        this.averageRecipeCost = data.averageRecipeCost;
        this.topExpensiveIngredients = data.topExpensiveIngredients;
        this.mostUsedIngredients = data.mostUsedIngredients;
        
        this.updateCharts();
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['/login']);
        }
      }
    });
  }

  private updateCharts(): void {
    // Atualizar dados do gráfico de custos
    this.costChartData = {
      ...this.costChartData,
      labels: this.topExpensiveIngredients.map(ing => ing.name),
      datasets: [{
        ...this.costChartData.datasets[0],
        data: this.topExpensiveIngredients.map(ing => ing.cost)
      }]
    };

    // Atualizar dados do gráfico de uso
    this.usageChartData = {
      ...this.usageChartData,
      labels: this.mostUsedIngredients.map(ing => ing.name),
      datasets: [{
        ...this.usageChartData.datasets[0],
        data: this.mostUsedIngredients.map(ing => ing.recipeCount)
      }]
    };

    // Forçar atualização dos gráficos
    if (this.chart) {
      this.chart.render();
      this.chart.update();
    }
  }
}
