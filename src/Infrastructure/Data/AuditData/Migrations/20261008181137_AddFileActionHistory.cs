using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditData.Migrations
{
    /// <inheritdoc />
    public partial class AddFileActionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit_store");

            migrationBuilder.CreateTable(
                name: "FileActionHistory",
                schema: "audit_store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    ActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SavedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileActionHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileActionHistory_FileId",
                schema: "audit_store",
                table: "FileActionHistory",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileActionHistory",
                schema: "audit_store");
        }
    }
}
