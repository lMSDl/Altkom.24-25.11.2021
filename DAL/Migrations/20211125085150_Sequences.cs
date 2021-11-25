using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Sequences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sequences");

            migrationBuilder.CreateSequence<int>(
                name: "ProductPrice",
                schema: "sequences",
                startValue: 10L,
                incrementBy: 4,
                minValue: 3L,
                maxValue: 55L,
                cyclic: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR sequences.ProductPrice",
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 10f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ProductPrice",
                schema: "sequences");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 10f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValueSql: "NEXT VALUE FOR sequences.ProductPrice");
        }
    }
}
