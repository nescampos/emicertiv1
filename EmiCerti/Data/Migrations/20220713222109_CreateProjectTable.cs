using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmiCerti.Data.Migrations
{
    public partial class CreateProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OffsetCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Certifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    TokenQuantity = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ReductionsPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReductionsPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReductionstCO2e = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
