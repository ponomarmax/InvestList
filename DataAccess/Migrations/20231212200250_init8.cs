using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactPersons");

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("1264fbe9-8738-4731-9e32-38bb8787a285"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("34ed4a2b-381b-435b-a29a-d9065401fd47"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("4d59f3bc-797a-46eb-89f1-753b2d6f44f1"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("71f2565c-c162-4966-8ef0-f18dc1b22fe8"));

            migrationBuilder.DeleteData(
                table: "InvestFields",
                keyColumn: "Id",
                keyValue: new Guid("738b603a-5de9-4259-ae1a-c480f903cb0d"));

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsToTags",
                columns: table => new
                {
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsToTags", x => new { x.NewsId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NewsToTags_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsToTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsToTags_TagId",
                table: "NewsToTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsToTags");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Tags");

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

            migrationBuilder.CreateTable(
                name: "ContactPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedInvestAdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactPersons_InvestAdExtraInfo_RelatedInvestAdId",
                        column: x => x.RelatedInvestAdId,
                        principalTable: "InvestAdExtraInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("1264fbe9-8738-4731-9e32-38bb8787a285"), "Лізинг Авто" },
                    { new Guid("34ed4a2b-381b-435b-a29a-d9065401fd47"), "Фінанси" },
                    { new Guid("4d59f3bc-797a-46eb-89f1-753b2d6f44f1"), "Займи" },
                    { new Guid("71f2565c-c162-4966-8ef0-f18dc1b22fe8"), "Кафе та ресторани" },
                    { new Guid("738b603a-5de9-4259-ae1a-c480f903cb0d"), "Сільськогосподарська техніка" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_RelatedInvestAdId",
                table: "ContactPersons",
                column: "RelatedInvestAdId");
        }
    }
}
