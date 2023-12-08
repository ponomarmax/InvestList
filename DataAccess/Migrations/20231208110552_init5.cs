using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("02c1ec3c-d5eb-4829-8d47-d767bab1b7af"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("147a5a19-3594-4545-a452-c6dbbd9d977a"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("93b3b897-3b79-439b-94bf-6493b9e01084"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("cce85f32-f0fe-46d1-944f-9dbca0809187"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("d9529268-ce27-413c-a8f7-d3bc2e439dfa"));

            migrationBuilder.DropColumn(
                name: "IsArchived",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "InvestAdExtraInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("02c1ec3c-d5eb-4829-8d47-d767bab1b7af"), "Сільськогосподарська техніка" },
                    { new Guid("147a5a19-3594-4545-a452-c6dbbd9d977a"), "Кафе та ресторани" },
                    { new Guid("93b3b897-3b79-439b-94bf-6493b9e01084"), "Фінанси" },
                    { new Guid("cce85f32-f0fe-46d1-944f-9dbca0809187"), "Займи" },
                    { new Guid("d9529268-ce27-413c-a8f7-d3bc2e439dfa"), "Лізинг Авто" }
                });
        }
    }
}
