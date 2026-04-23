using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultationRequestStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_handled",
                table: "consultation_request");

            migrationBuilder.AddColumn<int>(
                name: "consultation_request_status",
                table: "consultation_request",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "consultation_request_status",
                table: "consultation_request");

            migrationBuilder.AddColumn<bool>(
                name: "is_handled",
                table: "consultation_request",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
