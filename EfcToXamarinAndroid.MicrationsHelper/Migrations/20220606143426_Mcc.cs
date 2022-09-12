using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfcToXamarinAndroid.MigrationsHelper.Migrations
{
    public partial class Mcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HashId = table.Column<long>(nullable: false),
                    OperacionTyp = table.Column<int>(nullable: false),
                    Balance = table.Column<float>(nullable: false),
                    Sum = table.Column<float>(nullable: false),
                    Karta = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Descripton = table.Column<string>(nullable: true),
                    MCC = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DefaultCategoryTyps = table.Column<int>(nullable: false),
                    CastomCategoryTyps = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SybCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(nullable: false),
                    DataItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SybCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SybCategory_Cats_DataItemId",
                        column: x => x.DataItemId,
                        principalTable: "Cats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SybCategory_DataItemId",
                table: "SybCategory",
                column: "DataItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SybCategory");

            migrationBuilder.DropTable(
                name: "Cats");
        }
    }
}
