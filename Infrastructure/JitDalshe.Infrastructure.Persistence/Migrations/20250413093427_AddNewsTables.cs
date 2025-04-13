using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ext_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    publication_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "news_photo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ext_id = table.Column<long>(type: "bigint", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_news_photo_news_NewsId",
                        column: x => x.NewsId,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_primary_photo",
                columns: table => new
                {
                    news_id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_photo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_primary_photo", x => new { x.news_id, x.news_photo_id });
                    table.ForeignKey(
                        name: "FK_news_primary_photo_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_news_primary_photo_news_photo_news_photo_id",
                        column: x => x.news_photo_id,
                        principalTable: "news_photo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_news_ext_id",
                table: "news",
                column: "ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_photo_ext_id",
                table: "news_photo",
                column: "ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_photo_NewsId",
                table: "news_photo",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_news_primary_photo_news_id",
                table: "news_primary_photo",
                column: "news_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_primary_photo_news_photo_id",
                table: "news_primary_photo",
                column: "news_photo_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_primary_photo");

            migrationBuilder.DropTable(
                name: "news_photo");

            migrationBuilder.DropTable(
                name: "news");
        }
    }
}
