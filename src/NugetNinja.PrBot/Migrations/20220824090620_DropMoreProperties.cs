using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.NugetNinja.PrBot.Migrations
{
    public partial class DropMoreProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloneEndpoint",
                table: "Repos");

            migrationBuilder.DropColumn(
                name: "CloneToken",
                table: "Repos");

            migrationBuilder.DropColumn(
                name: "NugetPatToken",
                table: "Repos");

            migrationBuilder.DropColumn(
                name: "NugetServer",
                table: "Repos");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Repos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloneEndpoint",
                table: "Repos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CloneToken",
                table: "Repos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NugetPatToken",
                table: "Repos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NugetServer",
                table: "Repos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Provider",
                table: "Repos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
