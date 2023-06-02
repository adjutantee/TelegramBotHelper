using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotHelper.Migrations
{
    public partial class InitNewCommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_BlacklistOfWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordsName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BlacklistOfWords", x => x.Id);
                });

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
                table: "_BlacklistOfWords",
                columns: new[] { "Id", "WordsName" },
                values: new object[] { 1, "Stupid" });

            migrationBuilder.InsertData(
                table: "_CommandsName",
                columns: new[] { "Id", "CommandName" },
                values: new object[,]
                {
                    { 1, "/addblacklist" },
                    { 2, "/wordsinfo" },
                    { 3, "/deleteword" },
                    { 4, "/help" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_BlacklistOfWords");

            migrationBuilder.DropTable(
                name: "_CommandsName");
        }
    }
}
