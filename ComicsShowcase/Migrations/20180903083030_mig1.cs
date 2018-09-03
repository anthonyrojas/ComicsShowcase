using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsShowcase.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiskType",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "GraphicNovels",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "BookCondition",
                table: "GraphicNovels",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "Comics",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Conidition",
                table: "Comics",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ItemCategory",
                table: "Collectibles",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCategory",
                table: "Collectibles");

            migrationBuilder.AlterColumn<int>(
                name: "DiskType",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Publisher",
                table: "GraphicNovels",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "BookCondition",
                table: "GraphicNovels",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Publisher",
                table: "Comics",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Conidition",
                table: "Comics",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
