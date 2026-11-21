using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinationData.Migrations
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "data_store");

            migrationBuilder.CreateTable(
                name: "Fields",
                schema: "data_store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldCode = table.Column<int>(type: "integer", nullable: true),
                    FieldName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fields",
                schema: "data_store");
        }
    }
}
