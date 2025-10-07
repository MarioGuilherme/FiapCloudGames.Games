using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamesPurchases_GameGenres_GameGenreId",
                table: "GamesPurchases");

            migrationBuilder.DropIndex(
                name: "IX_GamesPurchases_GameGenreId",
                table: "GamesPurchases");

            migrationBuilder.DropColumn(
                name: "GameGenreId",
                table: "GamesPurchases");

            migrationBuilder.CreateTable(
                name: "GameGameGenre",
                columns: table => new
                {
                    GamesGameId = table.Column<int>(type: "int", nullable: false),
                    GenresGameGenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGameGenre", x => new { x.GamesGameId, x.GenresGameGenreId });
                    table.ForeignKey(
                        name: "FK_GameGameGenre_GameGenres_GenresGameGenreId",
                        column: x => x.GenresGameGenreId,
                        principalTable: "GameGenres",
                        principalColumn: "GameGenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGameGenre_Games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGameGenre_GenresGameGenreId",
                table: "GameGameGenre",
                column: "GenresGameGenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGameGenre");

            migrationBuilder.AddColumn<int>(
                name: "GameGenreId",
                table: "GamesPurchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GamesPurchases_GameGenreId",
                table: "GamesPurchases",
                column: "GameGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamesPurchases_GameGenres_GameGenreId",
                table: "GamesPurchases",
                column: "GameGenreId",
                principalTable: "GameGenres",
                principalColumn: "GameGenreId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
