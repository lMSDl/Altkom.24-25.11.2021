using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UpdateTime_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToExpire",
                table: "Product");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "dateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "dateTime",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "dateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "dateTime",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Order");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Product",
                type: "dateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Product",
                type: "dateTime",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Order",
                type: "dateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Order",
                type: "dateTime",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "DaysToExpire",
                table: "Product",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEDIFF(DAY, GETDATE(), [ExpirationDate])");
        }
    }
}
