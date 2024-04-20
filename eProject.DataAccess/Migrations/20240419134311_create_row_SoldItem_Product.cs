using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class create_row_SoldItem_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldItem",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldItem",
                table: "Product");
        }
    }
}
