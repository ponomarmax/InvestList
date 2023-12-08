using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("11c25857-c16d-4add-aeeb-9726a792c442"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("5f0d6dc1-7553-4583-8ae6-2fa78bb53afc"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("6bbacd01-75f7-46e1-a7e8-cbd79e43e315"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("dad5ed15-c88a-4365-9dc2-93c0c3106411"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("dddf8ab4-507c-4227-a4c0-5041b9b79aaf"));

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "InvestAdExtraInfo",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("171d2a86-74f7-4a76-a9b1-3ce7a7ea4fdf"), "Лізинг Авто" },
                    { new Guid("7d1fe758-0e09-4562-84c5-3efc1b872756"), "Фінанси" },
                    { new Guid("80180973-ff9a-4585-8438-8159f880990c"), "Займи" },
                    { new Guid("89973b11-d5f4-4a04-8b8e-52ad32e74bfb"), "Сільськогосподарська техніка" },
                    { new Guid("c2b9eabe-c3f6-4af9-9882-f509953be8f2"), "Кафе та ресторани" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("171d2a86-74f7-4a76-a9b1-3ce7a7ea4fdf"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7d1fe758-0e09-4562-84c5-3efc1b872756"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("80180973-ff9a-4585-8438-8159f880990c"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("89973b11-d5f4-4a04-8b8e-52ad32e74bfb"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("c2b9eabe-c3f6-4af9-9882-f509953be8f2"));

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "InvestAdExtraInfo");

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("11c25857-c16d-4add-aeeb-9726a792c442"), "Займи" },
                    { new Guid("5f0d6dc1-7553-4583-8ae6-2fa78bb53afc"), "Сільськогосподарська техніка" },
                    { new Guid("6bbacd01-75f7-46e1-a7e8-cbd79e43e315"), "Лізинг Авто" },
                    { new Guid("dad5ed15-c88a-4365-9dc2-93c0c3106411"), "Кафе та ресторани" },
                    { new Guid("dddf8ab4-507c-4227-a4c0-5041b9b79aaf"), "Фінанси" }
                });
        }
    }
}
