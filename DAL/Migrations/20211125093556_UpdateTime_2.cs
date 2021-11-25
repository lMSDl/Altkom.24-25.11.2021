using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UpdateTime_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysToExpire",
                table: "Product",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEDIFF(DAY, GETDATE(), [ExpirationDate])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToExpire",
                table: "Product");
        }
    }
}
