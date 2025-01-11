using RecipeMaster.Infra.IoC.JWT;
using RecipeMaster.Infra.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Infra.IoC.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapperSetup();
builder.Services.AddInfrastructure();
builder.Services.AddJwtSetup(builder.Configuration);
builder.Services.AddIdentitySetup();
builder.Services.AddDbContext<RecipeMasterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
