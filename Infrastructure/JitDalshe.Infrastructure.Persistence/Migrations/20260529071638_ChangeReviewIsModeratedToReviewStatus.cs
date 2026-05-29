using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReviewIsModeratedToReviewStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_moderated",
                table: "review");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "review",
                type: "text",
                nullable: false,
                defaultValue: "New");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "review");

            migrationBuilder.AddColumn<bool>(
                name: "is_moderated",
                table: "review",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
