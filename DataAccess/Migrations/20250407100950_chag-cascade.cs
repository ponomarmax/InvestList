using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class chagcascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinInvestValue_InvestPosts_InvestPostId",
                table: "MinInvestValue");
            

            migrationBuilder.AddForeignKey(
                name: "FK_MinInvestValue_InvestPosts_InvestPostId",
                table: "MinInvestValue",
                column: "InvestPostId",
                principalTable: "InvestPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinInvestValue_InvestPosts_InvestPostId",
                table: "MinInvestValue");

            migrationBuilder.AddForeignKey(
                name: "FK_MinInvestValue_InvestPosts_InvestPostId",
                table: "MinInvestValue",
                column: "InvestPostId",
                principalTable: "InvestPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
