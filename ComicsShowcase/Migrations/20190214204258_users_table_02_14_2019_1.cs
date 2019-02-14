using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsShowcase.Migrations
{
    public partial class users_table_02_14_2019_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthMonth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BirthDate",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "BirthMonth",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BirthYear",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
