using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHandledFieldToConsultationRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_handled",
                table: "consultation_request",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_handled",
                table: "consultation_request");
        }
    }
}
