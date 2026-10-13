using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditData.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileActionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                schema: "audit_store",
                table: "FileActionHistory");

            migrationBuilder.DropColumn(
                name: "EventName",
                schema: "audit_store",
                table: "FileActionHistory");

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "audit_store",
                table: "FileActionHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "audit_store",
                table: "FileActionHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                schema: "audit_store",
                table: "FileActionHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                schema: "audit_store",
                table: "FileActionHistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "audit_store",
                table: "FileActionHistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "audit_store",
                table: "FileActionHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
