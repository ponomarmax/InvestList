using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addpostlinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Posts_PostId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_PostId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Links");

            migrationBuilder.CreateTable(
                name: "PostLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnchorText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Follow = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLinks_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostLinks_PostId",
                table: "PostLinks",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostLinks");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Links",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_PostId",
                table: "Links",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Posts_PostId",
                table: "Links",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
