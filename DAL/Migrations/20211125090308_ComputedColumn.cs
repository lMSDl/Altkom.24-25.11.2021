using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ComputedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                computedColumnSql: "[Name] + ' ' + CONVERT(varchar, [Price]) + 'zł'",
                stored: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Product");
        }
    }
}
