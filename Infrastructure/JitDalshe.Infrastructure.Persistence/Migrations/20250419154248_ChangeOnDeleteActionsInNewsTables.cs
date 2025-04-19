using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_news_primary_photo_news_news_id",
                table: "news_primary_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_news_primary_photo_news_photo_news_photo_id",
                table: "news_primary_photo");

            migrationBuilder.AddForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_news_primary_photo_news_news_id",
                table: "news_primary_photo",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_news_primary_photo_news_photo_news_photo_id",
                table: "news_primary_photo",
                column: "news_photo_id",
                principalTable: "news_photo",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_news_primary_photo_news_news_id",
                table: "news_primary_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_news_primary_photo_news_photo_news_photo_id",
                table: "news_primary_photo");

            migrationBuilder.AddForeignKey(
                name: "FK_news_photo_news_news_id",
                table: "news_photo",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_news_primary_photo_news_news_id",
                table: "news_primary_photo",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_news_primary_photo_news_photo_news_photo_id",
                table: "news_primary_photo",
                column: "news_photo_id",
                principalTable: "news_photo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
