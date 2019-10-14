using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsShowcaseV3.Migrations
{
    public partial class initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    BirthDate = table.Column<string>(nullable: false),
                    ProfileStr = table.Column<string>(nullable: true),
                    Profile = table.Column<byte[]>(nullable: true),
                    redditUsername = table.Column<string>(nullable: true),
                    instagramUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Collectibles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ImageStr = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    UPC = table.Column<long>(nullable: false),
                    Autographed = table.Column<bool>(nullable: false),
                    ItemCategory = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collectibles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Collectibles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    UPC = table.Column<long>(nullable: false),
                    FiveDigitId = table.Column<int>(nullable: false),
                    ImageStr = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    Publisher = table.Column<string>(nullable: false),
                    Conidition = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comics_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphicNovels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ImageStr = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    GraphicNovelType = table.Column<int>(nullable: false),
                    BookCondition = table.Column<string>(nullable: false),
                    Publisher = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicNovels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GraphicNovels_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creators",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    ComicBookID = table.Column<int>(nullable: true),
                    GraphicNovelID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Creators_Comics_ComicBookID",
                        column: x => x.ComicBookID,
                        principalTable: "Comics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Creators_GraphicNovels_GraphicNovelID",
                        column: x => x.GraphicNovelID,
                        principalTable: "GraphicNovels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Creators_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collectibles_UserID",
                table: "Collectibles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Comics_UserID",
                table: "Comics",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Creators_ComicBookID",
                table: "Creators",
                column: "ComicBookID");

            migrationBuilder.CreateIndex(
                name: "IX_Creators_GraphicNovelID",
                table: "Creators",
                column: "GraphicNovelID");

            migrationBuilder.CreateIndex(
                name: "IX_Creators_UserID",
                table: "Creators",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicNovels_UserID",
                table: "GraphicNovels",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collectibles");

            migrationBuilder.DropTable(
                name: "Creators");

            migrationBuilder.DropTable(
                name: "Comics");

            migrationBuilder.DropTable(
                name: "GraphicNovels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
