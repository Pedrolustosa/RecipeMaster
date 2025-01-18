using RecipeMaster.Infra.IoC.JWT;
using RecipeMaster.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Infra.IoC.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();
builder.Services.AddAutoMapperSetup();
builder.Services.AddInfrastructure();
builder.Services.AddJwtSetup(builder.Configuration);
builder.Services.AddDbContext<RecipeMasterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure CORS
app.UseCors(opt => opt.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://localhost:4200", "https://localhost:4200", 
                                   "https://recipe-master-app.vercel.app", "http://recipe-master-app.vercel.app"));
// Use seed data
app.UseSeedData();

// Middleware for exception handling
app.UseMiddleware<ExceptionMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
