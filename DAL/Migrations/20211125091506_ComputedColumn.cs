using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ComputedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Product",
                type: "dateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DaysToExpire",
                table: "Product",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEDIFF(DAY, GETDATE(), [ExpirationDate])");

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
                name: "DaysToExpire",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Product");
        }
    }
}
