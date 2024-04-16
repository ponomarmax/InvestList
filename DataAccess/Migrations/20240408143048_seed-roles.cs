using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class seedroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[,]
                {
                    { "43f568be-9943-40b2-9afe-e4204fb622f8", "2d082636-24ca-4798-a1e6-7aaa33cb3c59", "business", "BUSINESS", null },
                    { "9ec17071-35bc-4910-ad12-ca4b5cef0ce5", "8d25da6b-f780-4062-8a63-d37775d7e319", "admin", "ADMIN", null }
                });

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("1b0483b4-c724-4e6f-a3cf-82e8bfe28b42"), "Фінанси" },
                    { new Guid("a29bacac-0b19-4543-b275-0fac79657308"), "Кафе та ресторани" },
                    { new Guid("b5d92a84-f627-44ea-bdcb-31f5eb1782ae"), "Лізинг Авто" },
                    { new Guid("bc972689-8b2a-48cc-91c9-501ab1282129"), "Сільськогосподарська техніка" },
                    { new Guid("bde5a537-9e5f-45b4-9d9e-21be47ef8de1"), "Займи" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0b4c3d4f-b3bf-472d-9109-993d0e000872"), "Сенсація" },
                    { new Guid("357e5238-d21d-460d-9cdc-045ff24fb0b7"), "Шахраї" },
                    { new Guid("c557d1ad-29d7-4c88-9a99-535f83d3802a"), "Цікавинка" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UserId",
                table: "AspNetRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_UserId",
                table: "AspNetRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_UserId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_UserId",
                table: "AspNetRoles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43f568be-9943-40b2-9afe-e4204fb622f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ec17071-35bc-4910-ad12-ca4b5cef0ce5");

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("1b0483b4-c724-4e6f-a3cf-82e8bfe28b42"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("a29bacac-0b19-4543-b275-0fac79657308"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("b5d92a84-f627-44ea-bdcb-31f5eb1782ae"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("bc972689-8b2a-48cc-91c9-501ab1282129"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("bde5a537-9e5f-45b4-9d9e-21be47ef8de1"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0b4c3d4f-b3bf-472d-9109-993d0e000872"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("357e5238-d21d-460d-9cdc-045ff24fb0b7"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c557d1ad-29d7-4c88-9a99-535f83d3802a"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetRoles");

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
    }
}
