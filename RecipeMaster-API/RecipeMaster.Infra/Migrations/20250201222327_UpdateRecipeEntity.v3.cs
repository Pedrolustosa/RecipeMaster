using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMaster.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeEntityv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipeName",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeName",
                table: "Recipes");
        }
    }
}
