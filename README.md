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

### Backend (.NET 8.0.11)

| Package | Version |
|---------|---------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.11 |
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 8.0.11 |
| Microsoft.AspNetCore.Identity | 2.1.39 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.11 |
| Microsoft.EntityFrameworkCore | 8.0.11 |
| Microsoft.EntityFrameworkCore.Design | 8.0.11 |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.11 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.11 |
| AutoMapper | 13.0.1 |
| FluentValidation | 11.11.0 |
| MediatR | 12.4.1 |
| Serilog.AspNetCore | 8.0.3 |
| Serilog.Extensions.Logging | 8.0.0 |
| Swashbuckle.AspNetCore | 7.2.0 |

### Frontend (Angular 18.2)

| Package | Version |
|---------|---------|
| @angular/animations | 18.2.13 |
| @angular/common | 18.2.13 |
| @angular/compiler | 18.2.13 |
| @angular/core | 18.2.13 |
| @angular/cdk | 18.2.13 |
| @angular/forms | 18.2.13 |
| @angular/platform-browser | 18.2.13 |
| @angular/platform-browser-dynamic | 18.2.13 |
| @angular/platform-server | 18.2.13 |
| @angular/router | 18.2.13 |
| @angular/ssr | 18.2.2 |
| @fortawesome/fontawesome-free | 6.5.1 |
| bootstrap | 5.3.3 |
| chart.js | 4.4.1 |
| ng2-charts | 4.0.0 |
| ngx-bootstrap | 18.1.3 |
| ngx-spinner | 17.0.0 |
| ngx-toastr | 19.0.0 |
| rxjs | 7.8.0 |
| zone.js | 0.14.10 |

## 🛠️ Installation

### Prerequisites
- .NET SDK 8.0 or higher
- Node.js 18.0 LTS or higher
- SQL Server 2022
- Angular CLI 18.2
- Git

### Backend Setup
```bash
# Clone the repository
git clone https://github.com/Pedrolustosa/RecipeMaster.git

# Navigate to the API directory
cd RecipeMaster/RecipeMaster-API

# Restore dependencies
dotnet restore

# Update database
dotnet ef database update

# Run the API
dotnet run --project RecipeMaster.API
```

### Frontend Setup
```bash
# Navigate to the Angular app directory
cd ../RecipeMaster-APP

# Install dependencies
npm install

# Start the development server
ng serve
```

The application will be available at:
- API: http://localhost:5000
- Frontend: http://localhost:4200
- Swagger Documentation: http://localhost:5000/swagger

## 🤝 How to Contribute

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.