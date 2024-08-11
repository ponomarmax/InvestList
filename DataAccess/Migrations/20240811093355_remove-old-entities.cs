using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeoldentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "InvestAdExtraInfoInvestField");

            migrationBuilder.DropTable(
                name: "InvestTags");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "MinimalInvestEntrance");

            migrationBuilder.DropTable(
                name: "NewsToTags");

            migrationBuilder.DropTable(
                name: "InvestFields");

            migrationBuilder.DropTable(
                name: "InvestAdExtraInfo");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "InvestAds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestAds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdateAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestAds_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSeo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleSeo = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "InvestAdExtraInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestAdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnualInvestmentReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestDurationMonths = table.Column<int>(type: "int", nullable: false),
                    InvestDurationYears = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestAdExtraInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestAdExtraInfo_InvestAds_InvestAdId",
                        column: x => x.InvestAdId,
                        principalTable: "InvestAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestTags",
                columns: table => new
                {
                    InvestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestTags", x => new { x.InvestId, x.TagId });
                    table.ForeignKey(
                        name: "FK_InvestTags_InvestAds_InvestId",
                        column: x => x.InvestId,
                        principalTable: "InvestAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestAdId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_InvestAds_InvestAdId",
                        column: x => x.InvestAdId,
                        principalTable: "InvestAds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnchorText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Follow = table.Column<bool>(type: "bit", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "InvestAdExtraInfoInvestField",
                columns: table => new
                {
                    InvestAdExtraInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestAdExtraInfoInvestField", x => new { x.InvestAdExtraInfoId, x.InvestFieldId });
                    table.ForeignKey(
                        name: "FK_InvestAdExtraInfoInvestField_InvestAdExtraInfo_InvestAdExtraInfoId",
                        column: x => x.InvestAdExtraInfoId,
                        principalTable: "InvestAdExtraInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestAdExtraInfoInvestField_InvestFields_InvestFieldId",
                        column: x => x.InvestFieldId,
                        principalTable: "InvestFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MinimalInvestEntrance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InvestAdExtraInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinimalInvestEntrance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinimalInvestEntrance_InvestAdExtraInfo_InvestAdExtraInfoId",
                        column: x => x.InvestAdExtraInfoId,
                        principalTable: "InvestAdExtraInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "InvestFields",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("072aa2c8-7641-4e36-af85-41d0cef7db4d"), "Спорт" },
                    { new Guid("07ba9b0e-aded-4706-ae0d-820d10cd2a7f"), "Авто" },
                    { new Guid("3ad6cd6e-fc51-4f55-a20b-304e208667b9"), "Освіта" },
                    { new Guid("42c6356d-6754-434e-ab92-99a6fdfd1d88"), "Займи" },
                    { new Guid("4ac89c0c-b3de-488f-a99b-42601585b9ac"), "Нерухомість в Україні" },
                    { new Guid("4f711134-52bb-4bbf-a2d2-e59eda732d67"), "Фінанси" },
                    { new Guid("60bdbcec-5716-4593-b3ea-3f4c080f03e4"), "Енергетика" },
                    { new Guid("6b38fcf2-47c5-41f2-878c-3a7c37072c55"), "Рітейл" },
                    { new Guid("7e3fa98e-9422-468d-8dcd-d66673583a76"), "Криптовалюти" },
                    { new Guid("8f4de586-06d7-45ed-bbbe-d6f732b02337"), "IT" },
                    { new Guid("92ee46d1-5461-4772-8dbd-0ef62a8a1d34"), "Сільськогосподарська техніка" },
                    { new Guid("9850f830-79cb-4e4c-8091-fa577047377d"), "Нерухомість закордоном" },
                    { new Guid("9d1c74fb-4160-46c7-bc4d-153a23ef39a3"), "Виробництво" },
                    { new Guid("bf6a5de8-1bbc-4367-9812-58eb3d1d7834"), "Агро" },
                    { new Guid("da5eab20-13aa-4f44-a30b-7be764dbcfbf"), "Кафе та ресторани" },
                    { new Guid("e9dc75b6-df9e-456f-9cde-0a4435391c90"), "Розваги" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InvestAdId",
                table: "Comments",
                column: "InvestAdId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NewsId",
                table: "Comments",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_PostId",
                table: "Image",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestAdExtraInfo_InvestAdId",
                table: "InvestAdExtraInfo",
                column: "InvestAdId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestAdExtraInfoInvestField_InvestFieldId",
                table: "InvestAdExtraInfoInvestField",
                column: "InvestFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestAds_AuthorId",
                table: "InvestAds",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestAds_Slug",
                table: "InvestAds",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_InvestTags_TagId",
                table: "InvestTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_NewsId",
                table: "Links",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimalInvestEntrance_InvestAdExtraInfoId",
                table: "MinimalInvestEntrance",
                column: "InvestAdExtraInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_News_Slug",
                table: "News",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_NewsToTags_TagId",
                table: "NewsToTags",
                column: "TagId");
        }
    }
}
