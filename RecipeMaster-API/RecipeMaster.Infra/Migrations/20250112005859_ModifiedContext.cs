using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMaster.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost_Value",
                table: "Ingredients",
                newName: "Cost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Ingredients",
                newName: "Cost_Value");
        }
    }
}
