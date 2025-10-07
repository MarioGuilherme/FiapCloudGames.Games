using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FiapCloudGames.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGameGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre");

            migrationBuilder.RenameTable(
                name: "GameGenre",
                newName: "GameGenres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGenres",
                table: "GameGenres",
                column: "GameGenreId");

            migrationBuilder.CreateTable(
                name: "GameGenresGames",
                columns: table => new
                {
                    GameGenreId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenresGames", x => new { x.GameGenreId, x.GameId });
                    table.ForeignKey(
                        name: "FK_GameGenresGames_GameGenres_GameGenreId",
                        column: x => x.GameGenreId,
                        principalTable: "GameGenres",
                        principalColumn: "GameGenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenresGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameGenres",
                columns: new[] { "GameGenreId", "Title" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Adventure" },
                    { 3, "Simulation" },
                    { 4, "Strategy" },
                    { 5, "Sports" },
                    { 6, "Racing" },
                    { 7, "Fight" },
                    { 8, "Shooter" },
                    { 9, "Platformer" },
                    { 10, "Puzzle" },
                    { 11, "Horror" },
                    { 12, "Stealth" },
                    { 13, "Sandbox" },
                    { 14, "MMORPG" },
                    { 15, "BattleRoyale" },
                    { 16, "MusicRhythm" },
                    { 17, "Indie" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenres_Title",
                table: "GameGenres",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameGenresGames_GameId",
                table: "GameGenresGames",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenresGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGenres",
                table: "GameGenres");

            migrationBuilder.DropIndex(
                name: "IX_GameGenres_Title",
                table: "GameGenres");

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "GameGenreId",
                keyValue: 17);

            migrationBuilder.RenameTable(
                name: "GameGenres",
                newName: "GameGenre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre",
                column: "GameGenreId");

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
                        name: "FK_GameGameGenre_GameGenre_GenresGameGenreId",
                        column: x => x.GenresGameGenreId,
                        principalTable: "GameGenre",
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
    }
}
