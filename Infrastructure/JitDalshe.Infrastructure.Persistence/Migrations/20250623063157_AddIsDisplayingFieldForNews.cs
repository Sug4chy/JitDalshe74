using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisplayingFieldForNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_displaying",
                table: "news",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_displaying",
                table: "news");
        }
    }
}
