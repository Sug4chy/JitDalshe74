using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixNamingIssuseWithNewsIdColumnInNewsPhotoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_photo_news_NewsId",
                table: "news_photo");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "news_photo",
                newName: "news_id");

            migrationBuilder.RenameIndex(
                name: "IX_news_photo_NewsId",
                table: "news_photo",
                newName: "IX_news_photo_news_id");

            migrationBuilder.AddForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "news_photo",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_news_photo_news_id",
                table: "news_photo",
                newName: "IX_news_photo_NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_news_photo_news_NewsId",
                table: "news_photo",
                column: "NewsId",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
