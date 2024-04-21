using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class addedtagsfornews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2090e53b-74ff-472a-bf38-5aa151a49b91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8c0a9944-3ee2-49b3-be23-ea3b9eae1e05"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8d5f713c-c098-4825-b6c0-48d81c5d3bd5"));

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("681e4bc1-de4e-42a7-98f6-4425476efb03"), "Інше" },
                    { new Guid("6a48934d-6459-4aaa-806a-594b4f05c7c3"), "SCUM" },
                    { new Guid("a4a1bb92-eb6a-4538-ae0d-dfcf70d0528c"), "Держрегулювання" },
                    { new Guid("d12cb617-608e-4109-85cb-af9f2cb95b6f"), "Рейтинги" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("681e4bc1-de4e-42a7-98f6-4425476efb03"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("6a48934d-6459-4aaa-806a-594b4f05c7c3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a4a1bb92-eb6a-4538-ae0d-dfcf70d0528c"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d12cb617-608e-4109-85cb-af9f2cb95b6f"));

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2090e53b-74ff-472a-bf38-5aa151a49b91"), "Шахраї" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8c0a9944-3ee2-49b3-be23-ea3b9eae1e05"), "Сенсація" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8d5f713c-c098-4825-b6c0-48d81c5d3bd5"), "Цікавинка" });
        }
    }
}
