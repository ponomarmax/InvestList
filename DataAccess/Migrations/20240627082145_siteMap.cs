using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class siteMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Posts_PostId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageMetadata_Posts_PostId",
                table: "ImageMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestPosts_Posts_PostId",
                table: "InvestPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLinks_Posts_PostId",
                table: "PostLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_Slug",
                table: "Post",
                newName: "IX_Post_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostType_IsActive",
                table: "Post",
                newName: "IX_Post_PostType_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CreatedById",
                table: "Post",
                newName: "IX_Post_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CreatedAt",
                table: "Post",
                newName: "IX_Post_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Post_PostId",
                table: "Image",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageMetadata_Post_PostId",
                table: "ImageMetadata",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestPosts_Post_PostId",
                table: "InvestPosts",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Post_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLinks_Post_PostId",
                table: "PostLinks",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Post_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Post_PostId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageMetadata_Post_PostId",
                table: "ImageMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestPosts_Post_PostId",
                table: "InvestPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Post_PostId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLinks_Post_PostId",
                table: "PostLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Post_PostId",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameIndex(
                name: "IX_Post_Slug",
                table: "Posts",
                newName: "IX_Posts_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PostType_IsActive",
                table: "Posts",
                newName: "IX_Posts_PostType_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CreatedById",
                table: "Posts",
                newName: "IX_Posts_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CreatedAt",
                table: "Posts",
                newName: "IX_Posts_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Posts_PostId",
                table: "Image",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageMetadata_Posts_PostId",
                table: "ImageMetadata",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestPosts_Posts_PostId",
                table: "InvestPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLinks_Posts_PostId",
                table: "PostLinks",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
