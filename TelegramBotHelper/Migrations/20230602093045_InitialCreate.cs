﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotHelper.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_CommandsName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CommandsName", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "_CommandsName",
                columns: new[] { "Id", "CommandName" },
                values: new object[] { 1, "/addblacklist" });

            migrationBuilder.InsertData(
                table: "_CommandsName",
                columns: new[] { "Id", "CommandName" },
                values: new object[] { 2, "/wordsinfo" });

            migrationBuilder.InsertData(
                table: "_CommandsName",
                columns: new[] { "Id", "CommandName" },
                values: new object[] { 3, "/deleteword" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_CommandsName");
        }
    }
}
