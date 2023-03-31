using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmiCerti.Data.Migrations
{
    public partial class HBARsentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HBARsent",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HBARsent",
                table: "Projects");
        }
    }
}
