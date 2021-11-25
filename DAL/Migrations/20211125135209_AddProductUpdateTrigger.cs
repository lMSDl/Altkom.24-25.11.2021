using DAL.Properties;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddProductUpdateTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.Updated);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
