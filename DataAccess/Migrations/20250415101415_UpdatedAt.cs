using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "ImageBytes",
            //     table: "TopPostWithInvestResult");
            //
            // migrationBuilder.AddColumn<DateTime>(
            //     name: "CreatedAt",
            //     table: "TopPostWithInvestResult",
            //     type: "datetime2",
            //     nullable: false,
            //     defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            //
            // migrationBuilder.AddColumn<Guid>(
            //     name: "TagId",
            //     table: "TopPostWithInvestResult",
            //     type: "uniqueidentifier",
            //     nullable: true);
            //
            // migrationBuilder.AddColumn<string>(
            //     name: "TagTitle",
            //     table: "TopPostWithInvestResult",
            //     type: "nvarchar(max)",
            //     nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts");

            // migrationBuilder.DropColumn(
            //     name: "CreatedAt",
            //     table: "TopPostWithInvestResult");
            //
            // migrationBuilder.DropColumn(
            //     name: "TagId",
            //     table: "TopPostWithInvestResult");
            //
            // migrationBuilder.DropColumn(
            //     name: "TagTitle",
            //     table: "TopPostWithInvestResult");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Posts");

            // migrationBuilder.AddColumn<byte[]>(
            //     name: "ImageBytes",
            //     table: "TopPostWithInvestResult",
            //     type: "varbinary(max)",
            //     nullable: true);
        }
    }
}
