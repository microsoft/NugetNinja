using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.NugetNinja.PrBot.Migrations
{
    public partial class AddOrgProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Org",
                table: "Repos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Org",
                table: "Repos");
        }
    }
}
