using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_SeoDetailscs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeoDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelativePagePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageH1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleAnalyticPostViews_PostId",
                table: "GoogleAnalyticPostViews",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeoDetails_RelativePagePath",
                table: "SeoDetails",
                column: "RelativePagePath");

            migrationBuilder.AddForeignKey(
                name: "FK_GoogleAnalyticPostViews_Posts_PostId",
                table: "GoogleAnalyticPostViews",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoogleAnalyticPostViews_Posts_PostId",
                table: "GoogleAnalyticPostViews");

            migrationBuilder.DropTable(
                name: "SeoDetails");

            migrationBuilder.DropIndex(
                name: "IX_GoogleAnalyticPostViews_PostId",
                table: "GoogleAnalyticPostViews");
        }
    }
}
