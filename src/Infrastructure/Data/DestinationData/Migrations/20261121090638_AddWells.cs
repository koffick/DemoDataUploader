using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinationData.Migrations
{
    /// <inheritdoc />
    public partial class AddWells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                schema: "data_store",
                table: "Organizations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Wells",
                schema: "data_store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WellName = table.Column<string>(type: "text", nullable: false),
                    WellNumber = table.Column<int>(type: "integer", nullable: true),
                    FieldId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cluster = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wells_Fields_FieldId",
                        column: x => x.FieldId,
                        principalSchema: "data_store",
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wells_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "data_store",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wells_FieldId",
                schema: "data_store",
                table: "Wells",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_OrganizationId",
                schema: "data_store",
                table: "Wells",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wells",
                schema: "data_store");

            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                schema: "data_store",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
