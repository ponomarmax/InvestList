using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class updateinvestfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7a6926ce-6ac8-4a41-85fa-708d9e26e27e"));

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("072aa2c8-7641-4e36-af85-41d0cef7db4d"), "Спорт" },
                    { new Guid("07ba9b0e-aded-4706-ae0d-820d10cd2a7f"), "Авто" },
                    { new Guid("3ad6cd6e-fc51-4f55-a20b-304e208667b9"), "Освіта" },
                    { new Guid("4ac89c0c-b3de-488f-a99b-42601585b9ac"), "Нерухомість в Україні" },
                    { new Guid("60bdbcec-5716-4593-b3ea-3f4c080f03e4"), "Енергетика" },
                    { new Guid("6b38fcf2-47c5-41f2-878c-3a7c37072c55"), "Рітейл" },
                    { new Guid("7e3fa98e-9422-468d-8dcd-d66673583a76"), "Криптовалюти" },
                    { new Guid("8f4de586-06d7-45ed-bbbe-d6f732b02337"), "IT" },
                    { new Guid("9850f830-79cb-4e4c-8091-fa577047377d"), "Нерухомість закордоном" },
                    { new Guid("9d1c74fb-4160-46c7-bc4d-153a23ef39a3"), "Виробництво" },
                    { new Guid("bf6a5de8-1bbc-4367-9812-58eb3d1d7834"), "Агро" },
                    { new Guid("e9dc75b6-df9e-456f-9cde-0a4435391c90"), "Розваги" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("072aa2c8-7641-4e36-af85-41d0cef7db4d"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("07ba9b0e-aded-4706-ae0d-820d10cd2a7f"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("3ad6cd6e-fc51-4f55-a20b-304e208667b9"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("4ac89c0c-b3de-488f-a99b-42601585b9ac"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("60bdbcec-5716-4593-b3ea-3f4c080f03e4"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("6b38fcf2-47c5-41f2-878c-3a7c37072c55"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7e3fa98e-9422-468d-8dcd-d66673583a76"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("8f4de586-06d7-45ed-bbbe-d6f732b02337"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("9850f830-79cb-4e4c-8091-fa577047377d"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("9d1c74fb-4160-46c7-bc4d-153a23ef39a3"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("bf6a5de8-1bbc-4367-9812-58eb3d1d7834"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("e9dc75b6-df9e-456f-9cde-0a4435391c90"));

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("7a6926ce-6ac8-4a41-85fa-708d9e26e27e"), "Лізинг Авто" });
        }
    }
}
