using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Joinrpg.Trelony.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacroRegions",
                columns: table => new
                {
                    MacroRegionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MacroRegionName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroRegions", x => x.MacroRegionId);
                });

            migrationBuilder.CreateTable(
                name: "SubRegions",
                columns: table => new
                {
                    SubRegionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MacroRegionId = table.Column<int>(nullable: false),
                    SubRegionName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubRegions", x => x.SubRegionId);
                    table.ForeignKey(
                        name: "FK_SubRegions_MacroRegions_MacroRegionId",
                        column: x => x.MacroRegionId,
                        principalTable: "MacroRegions",
                        principalColumn: "MacroRegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Polygons",
                columns: table => new
                {
                    PolygonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PolygonName = table.Column<string>(nullable: false),
                    SubRegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polygons", x => x.PolygonId);
                    table.ForeignKey(
                        name: "FK_Polygons_SubRegions_SubRegionId",
                        column: x => x.SubRegionId,
                        principalTable: "SubRegions",
                        principalColumn: "SubRegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    GameName = table.Column<string>(nullable: true),
                    GameStatus = table.Column<int>(nullable: false),
                    GameType = table.Column<int>(nullable: false),
                    GameUrl = table.Column<string>(nullable: true),
                    LivejournalLink = table.Column<string>(nullable: true),
                    Organizers = table.Column<string>(nullable: true),
                    PlayersCount = table.Column<int>(nullable: true),
                    PolygonId = table.Column<int>(nullable: true),
                    SubRegionId = table.Column<int>(nullable: false),
                    TelegramLink = table.Column<string>(nullable: true),
                    VkontakteLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Polygons_PolygonId",
                        column: x => x.PolygonId,
                        principalTable: "Polygons",
                        principalColumn: "PolygonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_SubRegions_SubRegionId",
                        column: x => x.SubRegionId,
                        principalTable: "SubRegions",
                        principalColumn: "SubRegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDate",
                columns: table => new
                {
                    GameDateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Actual = table.Column<bool>(nullable: false),
                    GameDurationDays = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    GameStartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDate", x => x.GameDateId);
                    table.ForeignKey(
                        name: "FK_GameDate_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameDate_GameId",
                table: "GameDate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PolygonId",
                table: "Games",
                column: "PolygonId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SubRegionId",
                table: "Games",
                column: "SubRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Polygons_SubRegionId",
                table: "Polygons",
                column: "SubRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegions_MacroRegionId",
                table: "SubRegions",
                column: "MacroRegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDate");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Polygons");

            migrationBuilder.DropTable(
                name: "SubRegions");

            migrationBuilder.DropTable(
                name: "MacroRegions");
        }
    }
}
