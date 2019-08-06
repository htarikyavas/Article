using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Data.Migrations
{
    public partial class migrAthrPsswrd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Author",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Author");
        }
    }
}
