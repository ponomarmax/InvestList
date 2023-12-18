using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("0f4cf0e9-6b8e-4bea-8cc3-11707e6abccb"), "Кафе та ресторани" },
                    { new Guid("328d780f-fec3-4564-838b-ed92388afc3b"), "Лізинг Авто" },
                    { new Guid("7320dcef-e544-451d-b415-4bc115c339a3"), "Займи" },
                    { new Guid("83b888a7-9bcb-49d3-b067-52f00883e8d7"), "Сільськогосподарська техніка" },
                    { new Guid("d8469604-e87e-4ef9-b845-39e4e5e8e874"), "Фінанси" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("17c6100f-28b2-4e65-ac47-d2271d5e53f3"), "Шахраї" },
                    { new Guid("d9030443-5c02-4836-b8c5-3c01cef45a75"), "Сенсація" },
                    { new Guid("dbc94b22-b28a-4e5d-8d2c-df8137080cf5"), "Цікавинка" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("0f4cf0e9-6b8e-4bea-8cc3-11707e6abccb"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("328d780f-fec3-4564-838b-ed92388afc3b"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7320dcef-e544-451d-b415-4bc115c339a3"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("83b888a7-9bcb-49d3-b067-52f00883e8d7"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("d8469604-e87e-4ef9-b845-39e4e5e8e874"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("17c6100f-28b2-4e65-ac47-d2271d5e53f3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d9030443-5c02-4836-b8c5-3c01cef45a75"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("dbc94b22-b28a-4e5d-8d2c-df8137080cf5"));

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
    }
}
