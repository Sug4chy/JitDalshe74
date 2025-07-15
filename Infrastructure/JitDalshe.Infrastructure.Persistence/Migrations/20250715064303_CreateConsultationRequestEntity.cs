using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateConsultationRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "consultation_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    patient_name = table.Column<string>(type: "text", nullable: false),
                    patient_age = table.Column<int>(type: "integer", nullable: false),
                    patient_phone_number = table.Column<string>(type: "text", nullable: true),
                    patient_email = table.Column<string>(type: "text", nullable: true),
                    communication_methods = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultation_request", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consultation_request");
        }
    }
}
