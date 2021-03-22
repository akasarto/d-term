using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dTerm.Infra.EfCore.Migrations
{
    public partial class AddDefaultConsoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Consoles",
                columns: new[] { "Id", "Icon", "Name", "OrderIndex", "ProcessExecutablePath", "ProcessStartupArgs", "ProcessType", "UTCCreation" },
                values: new object[] { new Guid("036c0c75-2509-4114-976e-0c123b80ad55"), "Console", "Command Promp", 1, "cmd.exe", "", (byte)1, new DateTime(2021, 3, 22, 1, 25, 22, 565, DateTimeKind.Utc).AddTicks(2020) });

            migrationBuilder.InsertData(
                table: "Consoles",
                columns: new[] { "Id", "Icon", "Name", "OrderIndex", "ProcessExecutablePath", "ProcessStartupArgs", "ProcessType", "UTCCreation" },
                values: new object[] { new Guid("5de39ac0-296f-423c-97f1-6ee71b7a4b2d"), "Git", "Git Bash", 2, "git-bash.exe", "--login -i", (byte)1, new DateTime(2021, 3, 22, 1, 25, 22, 565, DateTimeKind.Utc).AddTicks(2634) });

            migrationBuilder.InsertData(
                table: "Consoles",
                columns: new[] { "Id", "Icon", "Name", "OrderIndex", "ProcessExecutablePath", "ProcessStartupArgs", "ProcessType", "UTCCreation" },
                values: new object[] { new Guid("4e6d5d59-d537-4b66-aa97-1bd066dde1fe"), "Powershell", "Power Shell", 3, "powershell.exe", "", (byte)1, new DateTime(2021, 3, 22, 1, 25, 22, 565, DateTimeKind.Utc).AddTicks(2643) });

            migrationBuilder.InsertData(
                table: "Consoles",
                columns: new[] { "Id", "Icon", "Name", "OrderIndex", "ProcessExecutablePath", "ProcessStartupArgs", "ProcessType", "UTCCreation" },
                values: new object[] { new Guid("a460e3b0-278e-4328-bc5b-16f5a7eb1f27"), "Linux", "WSL Default Shel", 4, "wsl.exe", "", (byte)1, new DateTime(2021, 3, 22, 1, 25, 22, 565, DateTimeKind.Utc).AddTicks(2647) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Consoles",
                keyColumn: "Id",
                keyValue: new Guid("036c0c75-2509-4114-976e-0c123b80ad55"));

            migrationBuilder.DeleteData(
                table: "Consoles",
                keyColumn: "Id",
                keyValue: new Guid("4e6d5d59-d537-4b66-aa97-1bd066dde1fe"));

            migrationBuilder.DeleteData(
                table: "Consoles",
                keyColumn: "Id",
                keyValue: new Guid("5de39ac0-296f-423c-97f1-6ee71b7a4b2d"));

            migrationBuilder.DeleteData(
                table: "Consoles",
                keyColumn: "Id",
                keyValue: new Guid("a460e3b0-278e-4328-bc5b-16f5a7eb1f27"));
        }
    }
}
