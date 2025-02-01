using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMaster.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeEntityv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookingTime",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "YieldPerPortion",
                table: "Recipes",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "Recipes",
                newName: "QuantityPerProduction");

            migrationBuilder.RenameColumn(
                name: "Servings",
                table: "Recipes",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Recipes",
                newName: "ProductionCost");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "Recipes",
                newName: "YieldPerPortion");

            migrationBuilder.RenameColumn(
                name: "QuantityPerProduction",
                table: "Recipes",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Recipes",
                newName: "Servings");

            migrationBuilder.RenameColumn(
                name: "ProductionCost",
                table: "Recipes",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "CookingTime",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreparationTime",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
