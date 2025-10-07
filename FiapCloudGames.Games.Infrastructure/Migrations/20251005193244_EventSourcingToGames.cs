using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventSourcingToGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderUpdate");

            migrationBuilder.CreateTable(
                name: "GameEvent",
                columns: table => new
                {
                    GameEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EventAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 288, DateTimeKind.Local).AddTicks(1095))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvent", x => x.GameEventId);
                    table.ForeignKey(
                        name: "FK_GameEvent_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderEvent",
                columns: table => new
                {
                    OrderEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 289, DateTimeKind.Local).AddTicks(2196))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEvent", x => x.OrderEventId);
                    table.ForeignKey(
                        name: "FK_OrderEvent_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEvent_GameId",
                table: "GameEvent",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEvent_OrderId",
                table: "OrderEvent",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameEvent");

            migrationBuilder.DropTable(
                name: "OrderEvent");

            migrationBuilder.CreateTable(
                name: "OrderUpdate",
                columns: table => new
                {
                    OrderUpdateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 10, 5, 15, 50, 33, 957, DateTimeKind.Local).AddTicks(6217)),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderUpdate", x => x.OrderUpdateId);
                    table.ForeignKey(
                        name: "FK_OrderUpdate_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderUpdate_OrderId",
                table: "OrderUpdate",
                column: "OrderId");
        }
    }
}
