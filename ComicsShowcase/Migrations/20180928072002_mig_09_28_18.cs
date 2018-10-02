using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsShowcase.Migrations
{
    public partial class mig_09_28_18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "instagramUsername",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "redditUsername",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instagramUsername",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "redditUsername",
                table: "Users");
        }
    }
}
