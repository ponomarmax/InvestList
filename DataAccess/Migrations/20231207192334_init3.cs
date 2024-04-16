using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("44b944f7-155a-4483-ab2b-ae1b129dbe36"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("6cf99e4a-d0a7-40ac-be98-efba64a17776"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("8416d49c-a356-4133-bb23-bb2a75eacfae"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("bbc15a63-b666-4ea5-b2d6-9344dcede763"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("c7f51ba6-fef7-444d-a434-19590218a451"));

            migrationBuilder.DropColumn(
                name: "InvestPeriod",
                table: "InvestAdExtraInfo");

            migrationBuilder.AddColumn<int>(
                name: "InvestDurationMonths",
                table: "InvestAdExtraInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvestDurationYears",
                table: "InvestAdExtraInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "InvestDurationMonths",
                table: "InvestAdExtraInfo");

            migrationBuilder.DropColumn(
                name: "InvestDurationYears",
                table: "InvestAdExtraInfo");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "InvestPeriod",
                table: "InvestAdExtraInfo",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("44b944f7-155a-4483-ab2b-ae1b129dbe36"), "Кафе та ресторани" },
                    { new Guid("6cf99e4a-d0a7-40ac-be98-efba64a17776"), "Лізинг Авто" },
                    { new Guid("8416d49c-a356-4133-bb23-bb2a75eacfae"), "Займи" },
                    { new Guid("bbc15a63-b666-4ea5-b2d6-9344dcede763"), "Фінанси" },
                    { new Guid("c7f51ba6-fef7-444d-a434-19590218a451"), "Сільськогосподарська техніка" }
                });
        }
    }
}
