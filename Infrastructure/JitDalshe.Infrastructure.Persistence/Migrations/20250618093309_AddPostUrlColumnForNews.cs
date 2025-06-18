using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostUrlColumnForNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "post_url",
                table: "news",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "post_url",
                table: "news");
        }
    }
}
