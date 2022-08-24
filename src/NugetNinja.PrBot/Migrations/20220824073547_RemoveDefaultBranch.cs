using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.NugetNinja.PrBot.Migrations
{
    public partial class RemoveDefaultBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultBranch",
                table: "Repos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultBranch",
                table: "Repos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
