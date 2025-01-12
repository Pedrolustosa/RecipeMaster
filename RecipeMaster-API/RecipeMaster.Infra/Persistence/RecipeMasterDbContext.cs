using RecipeMaster.Core.Entities;
using RecipeMaster.Infra.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RecipeMaster.Infra.Persistence;

public class RecipeMasterDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public RecipeMasterDbContext(DbContextOptions<RecipeMasterDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany()
            .HasForeignKey(ri => ri.IngredientId);

        modelBuilder.Entity<Ingredient>()
            .OwnsOne(i => i.Cost, cost =>
            {
                cost.Property(c => c.Value).HasColumnName("Cost");
            });
    }
}
