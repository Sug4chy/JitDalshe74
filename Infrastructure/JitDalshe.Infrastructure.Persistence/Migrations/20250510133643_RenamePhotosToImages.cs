using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhotosToImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_photo");

            migrationBuilder.DropTable(
                name: "news_primary_photo");

            migrationBuilder.DropTable(
                name: "news_photo");

            migrationBuilder.CreateTable(
                name: "event_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_image_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ext_id = table.Column<long>(type: "bigint", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    news_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_news_image_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "news_primary_image",
                columns: table => new
                {
                    news_id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_image_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_primary_image", x => new { x.news_id, x.news_image_id });
                    table.ForeignKey(
                        name: "FK_news_primary_image_news_image_news_image_id",
                        column: x => x.news_image_id,
                        principalTable: "news_image",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_news_primary_image_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_image_event_id",
                table: "event_image",
                column: "event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_image_ext_id",
                table: "news_image",
                column: "ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_image_news_id",
                table: "news_image",
                column: "news_id");

            migrationBuilder.CreateIndex(
                name: "IX_news_primary_image_news_id",
                table: "news_primary_image",
                column: "news_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_primary_image_news_image_id",
                table: "news_primary_image",
                column: "news_image_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_image");

            migrationBuilder.DropTable(
                name: "news_primary_image");

            migrationBuilder.DropTable(
                name: "news_image");

            migrationBuilder.CreateTable(
                name: "event_photo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_photo_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_photo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ext_id = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_news_photo_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id");
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
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_news_primary_photo_news_photo_news_photo_id",
                        column: x => x.news_photo_id,
                        principalTable: "news_photo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_photo_event_id",
                table: "event_photo",
                column: "event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_photo_ext_id",
                table: "news_photo",
                column: "ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_photo_news_id",
                table: "news_photo",
                column: "news_id");

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
    }
}
