using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("0293bf97-1853-4d3a-9cc8-f6c150492d37"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("55e8624f-9091-4035-9a9d-eff82cc136b7"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("6fe24518-1878-4bae-bea6-624ebfb916d3"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("bfbcab05-a7bc-485a-815c-ef9fcc36d135"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("deb90418-0487-4f93-af1d-f7e436f713cb"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("0293bf97-1853-4d3a-9cc8-f6c150492d37"), "Кафе та ресторани" },
                    { new Guid("55e8624f-9091-4035-9a9d-eff82cc136b7"), "Лізинг Авто" },
                    { new Guid("6fe24518-1878-4bae-bea6-624ebfb916d3"), "Фінанси" },
                    { new Guid("bfbcab05-a7bc-485a-815c-ef9fcc36d135"), "Сільськогосподарська техніка" },
                    { new Guid("deb90418-0487-4f93-af1d-f7e436f713cb"), "Займи" }
                });
        }
    }
}
