using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("2cd6537a-d77c-440b-b395-3a80b2950fc3"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("96d73260-759c-4e9c-a7e2-88a4d23cec6f"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("970da07a-c915-4d9a-9a01-da9776e13bc0"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("a195c1d9-2ff8-40e9-bf44-697fe1653015"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("e10f00b6-c107-4235-9bcd-70e9d473ed0d"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("4043bc2e-eb3d-4ab3-a6c7-8390df438a75"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("5e5ebceb-7895-4805-aa2c-45831bcd9f04"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("f23ae485-6f70-44e4-b143-f11f1348108c"));

            migrationBuilder.AddColumn<string>(
                name: "ImageBase64",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("0ba89160-2f5a-41eb-9ecf-98377e279b5b"), "Кафе та ресторани" },
                    { new Guid("0df3b7ac-5aa8-4c4e-8a3f-3128276871ad"), "Лізинг Авто" },
                    { new Guid("3835fb5e-90ed-4162-ab9d-b56889ff9d96"), "Фінанси" },
                    { new Guid("6f629665-d2cf-4645-bcb3-a3bf9d43442f"), "Сільськогосподарська техніка" },
                    { new Guid("f21c5b17-da09-41b4-a4fb-579162033d8c"), "Займи" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("121a3cba-1283-46c7-9510-232710fe10e0"), "Сенсація" },
                    { new Guid("2016344b-6701-40c9-ae44-c9054d60edf9"), "Шахраї" },
                    { new Guid("dd36c733-469c-4bf4-a0e3-b9c31230c2c2"), "Цікавинка" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("0ba89160-2f5a-41eb-9ecf-98377e279b5b"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("0df3b7ac-5aa8-4c4e-8a3f-3128276871ad"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("3835fb5e-90ed-4162-ab9d-b56889ff9d96"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("6f629665-d2cf-4645-bcb3-a3bf9d43442f"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("f21c5b17-da09-41b4-a4fb-579162033d8c"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("121a3cba-1283-46c7-9510-232710fe10e0"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2016344b-6701-40c9-ae44-c9054d60edf9"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("dd36c733-469c-4bf4-a0e3-b9c31230c2c2"));

            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "News");

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("2cd6537a-d77c-440b-b395-3a80b2950fc3"), "Займи" },
                    { new Guid("96d73260-759c-4e9c-a7e2-88a4d23cec6f"), "Лізинг Авто" },
                    { new Guid("970da07a-c915-4d9a-9a01-da9776e13bc0"), "Кафе та ресторани" },
                    { new Guid("a195c1d9-2ff8-40e9-bf44-697fe1653015"), "Фінанси" },
                    { new Guid("e10f00b6-c107-4235-9bcd-70e9d473ed0d"), "Сільськогосподарська техніка" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4043bc2e-eb3d-4ab3-a6c7-8390df438a75"), "Цікавинка" },
                    { new Guid("5e5ebceb-7895-4805-aa2c-45831bcd9f04"), "Шахраї" },
                    { new Guid("f23ae485-6f70-44e4-b143-f11f1348108c"), "Сенсація" }
                });
        }
    }
}
