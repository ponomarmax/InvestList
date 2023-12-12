using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ImageBase64",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("1264fbe9-8738-4731-9e32-38bb8787a285"), "Лізинг Авто" },
                    { new Guid("34ed4a2b-381b-435b-a29a-d9065401fd47"), "Фінанси" },
                    { new Guid("4d59f3bc-797a-46eb-89f1-753b2d6f44f1"), "Займи" },
                    { new Guid("71f2565c-c162-4966-8ef0-f18dc1b22fe8"), "Кафе та ресторани" },
                    { new Guid("738b603a-5de9-4259-ae1a-c480f903cb0d"), "Сільськогосподарська техніка" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("1264fbe9-8738-4731-9e32-38bb8787a285"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("34ed4a2b-381b-435b-a29a-d9065401fd47"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("4d59f3bc-797a-46eb-89f1-753b2d6f44f1"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("71f2565c-c162-4966-8ef0-f18dc1b22fe8"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("738b603a-5de9-4259-ae1a-c480f903cb0d"));

            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "InvestAdExtraInfo");

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
    }
}
