using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditData.Migrations
{
    /// <inheritdoc />
    public partial class AddUserActionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActionHistory",
                schema: "audit_store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    ActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SavedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActionHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActionHistory_FileId",
                schema: "audit_store",
                table: "UserActionHistory",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActionHistory_UserId",
                schema: "audit_store",
                table: "UserActionHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActionHistory",
                schema: "audit_store");
        }
    }
}
