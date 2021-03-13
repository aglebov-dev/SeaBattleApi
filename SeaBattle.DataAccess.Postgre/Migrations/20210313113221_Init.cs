using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SeaBattle.DataAccess.Postgre.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "private");

            migrationBuilder.CreateTable(
                name: "games",
                schema: "private",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    size = table.Column<int>(type: "integer", nullable: false),
                    init = table.Column<bool>(type: "boolean", nullable: false),
                    ended = table.Column<bool>(type: "boolean", nullable: false),
                    finished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ships",
                schema: "private",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    x_start = table.Column<int>(type: "integer", nullable: false),
                    x_end = table.Column<int>(type: "integer", nullable: false),
                    y_start = table.Column<int>(type: "integer", nullable: false),
                    y_end = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ships", x => x.id);
                    table.ForeignKey(
                        name: "FK_ships_games_game_id",
                        column: x => x.game_id,
                        principalSchema: "private",
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shots",
                schema: "private",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    ship_id = table.Column<long>(type: "bigint", nullable: true),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shots", x => x.id);
                    table.ForeignKey(
                        name: "FK_shots_games_game_id",
                        column: x => x.game_id,
                        principalSchema: "private",
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shots_ships_ship_id",
                        column: x => x.ship_id,
                        principalSchema: "private",
                        principalTable: "ships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ships_game_id",
                schema: "private",
                table: "ships",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_shots_game_id",
                schema: "private",
                table: "shots",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_shots_ship_id",
                schema: "private",
                table: "shots",
                column: "ship_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shots",
                schema: "private");

            migrationBuilder.DropTable(
                name: "ships",
                schema: "private");

            migrationBuilder.DropTable(
                name: "games",
                schema: "private");
        }
    }
}
