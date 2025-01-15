# 👨‍🍳 RecipeMaster

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Angular](https://img.shields.io/badge/Angular-18.2.13-DD0031?style=flat-square&logo=angular)](https://angular.io)
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9-3178C6?style=flat-square&logo=typescript)](https://www.typescriptlang.org)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.3-7952B3?style=flat-square&logo=bootstrap)](https://getbootstrap.com)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Status](https://img.shields.io/badge/Status-In%20Development-green?style=flat-square)]()

</div>

## 📋 Summary

- [About](#-about)
- [Technologies](#-technologies)
- [Installation](#️-installation)
- [How to Contribute](#-how-to-contribute)
- [License](#-license)

## 📝 About

RecipeMaster is a full-stack application for managing culinary recipes, offering a modern and intuitive experience to create, share, and discover recipes.

### ✨ Highlights

- 📱 Responsive and adaptive design
- 🔒 Robust authentication system
- 🌐 RESTful API with Swagger documentation

## 🚀 Technologies

### Backend (.NET 8.0)

```csharp
RecipeMaster.API/                  // Presentation Layer
├── Controllers/                   // API Controllers
│   ├── AuthController.cs
│   ├── IngredientController.cs
│   └── RecipeController.cs
├── Exceptions/                    // Exception Handling
│   ├── BadRequestException.cs
│   ├── BaseException.cs
│   ├── InternalServerException.cs
│   ├── NotFoundException.cs
│   ├── UnauthorizedException.cs
│   └── ValidationException.cs
├── Middlewares/                   // API Middlewares
│   └── ExceptionMiddleware.cs
├── Models/                        // API Models
│   ├── LoginRequest.cs
│   ├── RegisterRequest.cs
│   └── TokenResponse.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
└── Program.cs

RecipeMaster.Application/          // Application Layer
├── Commands/                      // CQRS Commands
│   ├── Ingredient/
│   │   ├── CreateIngredientCommand.cs
│   │   ├── DeleteIngredientCommand.cs
│   │   └── UpdateIngredientCommand.cs
│   └── Recipe/
│       ├── CreateRecipeCommand.cs
│       ├── DeleteRecipeCommand.cs
│       └── UpdateRecipeCommand.cs
├── DTOs/                         // Data Transfer Objects
│   ├── IngredientDTO.cs
│   ├── RecipeDTO.cs
│   ├── RecipeIngredientDTO.cs
│   ├── UpdateIngredientDTO.cs
│   ├── UpdateRecipeDTO.cs
│   └── UpdateRecipeIngredientDTO.cs
├── Handlers/                     // CQRS Handlers
│   ├── Ingredient/
│   │   ├── CreateIngredientHandler.cs
│   │   ├── DeleteIngredientHandler.cs
│   │   ├── GetIngredientHandler.cs
│   │   ├── GetIngredientsHandler.cs
│   │   └── UpdateIngredientHandler.cs
│   └── Recipe/
│       ├── CreateRecipeHandler.cs
│       ├── DeleteRecipeHandler.cs
│       ├── GetRecipeHandler.cs
│       ├── GetRecipesHandler.cs
│       └── UpdateRecipeHandler.cs
├── Queries/                      // CQRS Queries
│   ├── Ingredient/
│   │   ├── GetIngredientQuery.cs
│   │   └── GetIngredientsQuery.cs
│   └── Recipe/
│       ├── GetRecipeQuery.cs
│       └── GetRecipesQuery.cs
├── Mappings/                     // AutoMapper Profiles
│   └── MappingProfile.cs
└── Services/                     // Application Services
    └── CostCalculationService.cs

RecipeMaster.Core/                // Domain Layer
├── Entities/                     // Entities
│   ├── Ingredient.cs            // Ingredient Entity
│   ├── Recipe.cs                // Recipe Entity
│   └── RecipeIngredient.cs      // M:N Relationship
├── Interfaces/                   // Interfaces
│   ├── Repositories/            // Repositories
│   │   ├── IIngredientRepository.cs
│   │   └── IRecipeRepository.cs
│   └── Services/                // Services
│       └── ICostCalculationService.cs
└── ValueObjects/                 // Value Objects
    ├── IngredientCost.cs        // Ingredient Cost
    └── MeasurementUnit.cs       // Measurement Unit

RecipeMaster.Infra/               // Infrastructure Layer
├── Identity/                     // Identity
│   ├── ApplicationRole.cs
│   ├── ApplicationUser.cs
│   └── IdentitySetup.cs
├── Migrations/                   // Migrations
├── Persistence/                  // Persistence
│   ├── RecipeMasterDbContext.cs
│   └── SeedData.cs
└── Repositories/                 // Repositories
    ├── IngredientRepository.cs
    └── RecipeRepository.cs

RecipeMaster.Infra.IoC/           // Dependency Injection
├── Configurations/               // Configurations
│   ├── AddBearerTokenDefaultValueFilter.cs
│   ├── AutoMapperSetup.cs
│   ├── DependencyInjection.cs
│   ├── SeedDataSetup.cs
│   └── SwaggerSetup.cs
└── JWT/                         // JWT Configuration
    ├── JwtSettings.cs
    └── JwtSetup.cs
```

### Frontend (Angular 18.2)

```typescript
// Main Structure
src/
├── app/
│   ├── components/
│   │   ├── dashboard/           // Main Dashboard
│   │   │   ├── dashboard.component.ts
│   │   │   ├── dashboard.component.html
│   │   │   └── dashboard.component.css
│   │   ├── ingredient/          // Ingredient Management
│   │   │   ├── ingredient-list/
│   │   │   ├── ingredient-form/
│   │   │   └── ingredient-detail/
│   │   ├── login/              // Authentication
│   │   │   ├── login.component.ts
│   │   │   ├── login.component.html
│   │   │   └── login.component.css
│   │   └── shared/             // Shared Components
│   │       └── sidebar/        // Sidebar Menu
│   │           ├── sidebar.component.ts
│   │           ├── sidebar.component.html
│   │           └── sidebar.component.css
│   ├── guards/                 // Route Protection
│   │   └── auth.guard.ts
│   ├── interceptors/          // HTTP Interceptors
│   │   └── token.interceptor.ts
│   ├── models/               // Interfaces
│   │   ├── ingredient.model.ts
│   │   └── user.model.ts
│   ├── services/            // Services
│   │   ├── auth.service.ts
│   │   └── ingredient.service.ts
│   └── app.routes.ts        // Application Routes
```

#### Main Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| @angular/* | 18.2.13 | Framework Core |
| bootstrap | 5.3.3 | UI Framework |
| @fortawesome/fontawesome-free | 6.5.1 | Icons |
| ngx-bootstrap | 18.1.3 | Components |
| ngx-toastr | 19.0.0 | Notifications |

## 🛠️ Installation

### Requirements

```bash
# Required Versions
.NET SDK >= 8.0
Node.js >= 18.0 LTS
SQL Server >= 2019
Angular CLI >= 18.2
```

### Configuration

```bash
# Clone the repository
git clone https://github.com/your-username/RecipeMaster.git
cd RecipeMaster

# Backend
cd RecipeMaster-API
dotnet restore
dotnet ef database update
dotnet run

# Frontend
cd ../RecipeMaster-APP
npm install
ng serve
```

Access `http://localhost:4200`

## 🤝 How to Contribute

```bash
# Contribution Process
1. Fork (https://github.com/your-username/RecipeMaster/fork)
2. git checkout -b feature/MyFeature
3. git commit -m 'Add new feature'
4. git push origin feature/MyFeature
5. Create Pull Request
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

<div align="center">

</div>