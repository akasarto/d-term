using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dTerm.Infra.EfCore.Migrations
{
    public partial class CreateInitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessExecutablePath = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ProcessStartupArgs = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ProcessType = table.Column<byte>(type: "INTEGER", nullable: false),
                    UTCCreation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consoles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consoles");
        }
    }
}
