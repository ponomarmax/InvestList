using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class extendinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasChrome",
                table: "UserRequestInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MouseMoved",
                table: "UserRequestInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NavigatorWebdriver",
                table: "UserRequestInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ScreenHeight",
                table: "UserRequestInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScreenWidth",
                table: "UserRequestInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeSpent",
                table: "UserRequestInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasChrome",
                table: "UserRequestInfos");

            migrationBuilder.DropColumn(
                name: "MouseMoved",
                table: "UserRequestInfos");

            migrationBuilder.DropColumn(
                name: "NavigatorWebdriver",
                table: "UserRequestInfos");

            migrationBuilder.DropColumn(
                name: "ScreenHeight",
                table: "UserRequestInfos");

            migrationBuilder.DropColumn(
                name: "ScreenWidth",
                table: "UserRequestInfos");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "UserRequestInfos");
        }
    }
}
