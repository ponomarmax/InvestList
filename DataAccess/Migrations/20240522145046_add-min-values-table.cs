using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addminvaluestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinimalInvestEntrance_InvestPosts_InvestPostId",
                table: "MinimalInvestEntrance");

            migrationBuilder.DropIndex(
                name: "IX_MinimalInvestEntrance_InvestPostId",
                table: "MinimalInvestEntrance");

            migrationBuilder.DropColumn(
                name: "InvestPostId",
                table: "MinimalInvestEntrance");

            migrationBuilder.CreateTable(
                name: "MinInvestValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinInvestValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinInvestValue_InvestPosts_InvestPostId",
                        column: x => x.InvestPostId,
                        principalTable: "InvestPosts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MinInvestValue_InvestPostId",
                table: "MinInvestValue",
                column: "InvestPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinInvestValue");

            migrationBuilder.AddColumn<Guid>(
                name: "InvestPostId",
                table: "MinimalInvestEntrance",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinimalInvestEntrance_InvestPostId",
                table: "MinimalInvestEntrance",
                column: "InvestPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimalInvestEntrance_InvestPosts_InvestPostId",
                table: "MinimalInvestEntrance",
                column: "InvestPostId",
                principalTable: "InvestPosts",
                principalColumn: "Id");
        }
    }
}
