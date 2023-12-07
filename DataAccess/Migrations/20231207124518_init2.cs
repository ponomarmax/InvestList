using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("0a2020e6-3076-4f8b-be00-8f7f7d16d995"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("8f4e8da4-0839-4363-9c0c-ea8091c9ec94"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("a3487350-2bb1-46dd-8617-489f60522c8f"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("c562bb0c-4982-470b-acf4-77f03d09a75b"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("e62a727b-9cb4-4189-955d-c0e3d1692435"));

            migrationBuilder.AlterColumn<string>(
                name: "SpendInvestDesc",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfitPaymentScheme",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OtherInfo",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("442fc1dc-ec16-4af7-960f-5f6bf984c65a"), "Кафе та ресторани" },
                    { new Guid("7e054b12-9fa4-4e76-92fe-b2de41e1f35c"), "Лізинг Авто" },
                    { new Guid("9fbe0909-6d8c-426d-bdc5-d82ae465e88c"), "Фінанси" },
                    { new Guid("ec3970fc-551a-436a-9d86-fbb4cb24d635"), "Сільськогосподарська техніка" },
                    { new Guid("f7475521-afc1-4181-af30-1074d3f3dfc0"), "Займи" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("442fc1dc-ec16-4af7-960f-5f6bf984c65a"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("7e054b12-9fa4-4e76-92fe-b2de41e1f35c"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("9fbe0909-6d8c-426d-bdc5-d82ae465e88c"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("ec3970fc-551a-436a-9d86-fbb4cb24d635"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("f7475521-afc1-4181-af30-1074d3f3dfc0"));

            migrationBuilder.AlterColumn<string>(
                name: "SpendInvestDesc",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfitPaymentScheme",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OtherInfo",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InvestAdExtraInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("0a2020e6-3076-4f8b-be00-8f7f7d16d995"), "Займи" },
                    { new Guid("8f4e8da4-0839-4363-9c0c-ea8091c9ec94"), "Фінанси" },
                    { new Guid("a3487350-2bb1-46dd-8617-489f60522c8f"), "Кафе та ресторани" },
                    { new Guid("c562bb0c-4982-470b-acf4-77f03d09a75b"), "Лізинг Авто" },
                    { new Guid("e62a727b-9cb4-4189-955d-c0e3d1692435"), "Сільськогосподарська техніка" }
                });
        }
    }
}
