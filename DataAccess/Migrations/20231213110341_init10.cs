using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "OtherInfo",
                table: "InvestAdExtraInfo");

            migrationBuilder.DropColumn(
                name: "ProfitPaymentScheme",
                table: "InvestAdExtraInfo");

            migrationBuilder.DropColumn(
                name: "SpendInvestDesc",
                table: "InvestAdExtraInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "AnnualInvestmentReturn",
                table: "InvestAdExtraInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("03f267a3-8e9f-4b22-909c-424f62488809"), "Кафе та ресторани" },
                    { new Guid("679be741-22ab-4a35-b28b-3ca22d3f5c0f"), "Лізинг Авто" },
                    { new Guid("7cbcde05-1939-4ff0-8424-42a36afc08fa"), "Сільськогосподарська техніка" },
                    { new Guid("d46ad159-6fa9-48ec-86aa-01376b9efbe1"), "Займи" },
                    { new Guid("e1bec609-b47c-4458-b7c8-88d7fea60c14"), "Фінанси" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2b0c288d-6079-4a73-a7a8-e2012f35de7a"), "Сенсація" },
                    { new Guid("598d4c51-67b2-48f1-ad5d-3c9806273736"), "Цікавинка" },
                    { new Guid("bf937e43-f013-4593-a6ce-320d67bdab08"), "Шахраї" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("03f267a3-8e9f-4b22-909c-424f62488809"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("679be741-22ab-4a35-b28b-3ca22d3f5c0f"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7cbcde05-1939-4ff0-8424-42a36afc08fa"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("d46ad159-6fa9-48ec-86aa-01376b9efbe1"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("e1bec609-b47c-4458-b7c8-88d7fea60c14"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2b0c288d-6079-4a73-a7a8-e2012f35de7a"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("598d4c51-67b2-48f1-ad5d-3c9806273736"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("bf937e43-f013-4593-a6ce-320d67bdab08"));

            migrationBuilder.DropColumn(
                name: "AnnualInvestmentReturn",
                table: "InvestAdExtraInfo");

            migrationBuilder.AddColumn<string>(
                name: "OtherInfo",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfitPaymentScheme",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpendInvestDesc",
                table: "InvestAdExtraInfo",
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
    }
}
