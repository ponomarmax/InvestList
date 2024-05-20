using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addslug: Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "News",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "InvestAds",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_News_Slug",
                table: "News",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_InvestAds_Slug",
                table: "InvestAds",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_News_Slug",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_InvestAds_Slug",
                table: "InvestAds");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "InvestAds");
        }
    }
}
