using RecipeMaster.Infra.IoC.JWT;
using RecipeMaster.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Infra.IoC.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();
builder.Services.AddAutoMapperSetup();
builder.Services.AddInfrastructure();
builder.Services.AddJwtSetup(builder.Configuration);

// Configure DbContext
//builder.Services.AddDbContext<RecipeMasterDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<RecipeMasterDbContext>(options =>
    options.UseInMemoryDatabase("RecipeMasterDb"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins(
                  "http://localhost:4200",
                  "https://localhost:4200",
                  "https://recipe-master-app.vercel.app",
                  "http://recipe-master-app.vercel.app"
              );
    });
});

var app = builder.Build();

// Apply middleware in the correct order

// Middleware for CORS
app.UseCors("AllowSpecificOrigins");

// Seed initial data if applicable
app.UseSeedData();

// Middleware for exception handling
app.UseMiddleware<ExceptionMiddleware>();

// Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI();

// Enforce HTTPS redirection
app.UseHttpsRedirection();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
