using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitDalshe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIsDisplayingByStatusAndAddNullableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_displaying",
                table: "event");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "event",
                newName: "short_description");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "date",
                table: "event",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "full_text",
                table: "event",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "event",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "event",
                type: "text",
                nullable: false,
                defaultValue: "NotPublished");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "time",
                table: "event",
                type: "time without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_text",
                table: "event");

            migrationBuilder.DropColumn(
                name: "location",
                table: "event");

            migrationBuilder.DropColumn(
                name: "status",
                table: "event");

            migrationBuilder.DropColumn(
                name: "time",
                table: "event");

            migrationBuilder.RenameColumn(
                name: "short_description",
                table: "event",
                newName: "description");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "date",
                table: "event",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_displaying",
                table: "event",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
