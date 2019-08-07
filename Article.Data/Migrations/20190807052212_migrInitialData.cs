using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Article.Data.Migrations
{
    public partial class migrInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "Author",
           columns: new[] { "Name", "Email", "Password", "CreatedAt" },
           values: new object[] { "Hasan Tarık YAVAŞ", "htarikyavas@gmail.com", "123456", DateTime.Now });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
