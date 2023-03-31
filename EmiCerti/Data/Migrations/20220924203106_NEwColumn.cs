using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmiCerti.Data.Migrations
{
    public partial class NEwColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "signatureRequestId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "signatureRequestId",
                table: "Projects");
        }
    }
}
