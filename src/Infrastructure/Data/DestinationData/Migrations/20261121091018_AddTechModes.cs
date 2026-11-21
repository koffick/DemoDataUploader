using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinationData.Migrations
{
    /// <inheritdoc />
    public partial class AddTechModes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechModes",
                schema: "data_store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportingMonth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WellId = table.Column<Guid>(type: "uuid", nullable: false),
                    WellType = table.Column<string>(type: "text", nullable: false),
                    Layers = table.Column<string>(type: "text", nullable: false),
                    ExploitationMode = table.Column<string>(type: "text", nullable: false),
                    PumpType = table.Column<string>(type: "text", nullable: false),
                    F = table.Column<double>(type: "double precision", nullable: true),
                    Hsp = table.Column<double>(type: "double precision", nullable: true),
                    HspMax = table.Column<double>(type: "double precision", nullable: true),
                    BottomholeDepth = table.Column<double>(type: "double precision", nullable: true),
                    Pzatr = table.Column<double>(type: "double precision", nullable: true),
                    Pin = table.Column<double>(type: "double precision", nullable: true),
                    DynLevel = table.Column<double>(type: "double precision", nullable: true),
                    OilRate = table.Column<double>(type: "double precision", nullable: true),
                    LiqRate = table.Column<double>(type: "double precision", nullable: true),
                    WaterCut = table.Column<double>(type: "double precision", nullable: true),
                    OilDensity = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechModes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechModes_Wells_WellId",
                        column: x => x.WellId,
                        principalSchema: "data_store",
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechModes_WellId",
                schema: "data_store",
                table: "TechModes",
                column: "WellId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechModes",
                schema: "data_store");
        }
    }
}
