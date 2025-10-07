using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameTableInPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvent_Games_GameId",
                table: "GameEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderEvent_Orders_OrderId",
                table: "OrderEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderEvent",
                table: "OrderEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEvent",
                table: "GameEvent");

            migrationBuilder.RenameTable(
                name: "OrderEvent",
                newName: "OrderEvents");

            migrationBuilder.RenameTable(
                name: "GameEvent",
                newName: "GameEvents");

            migrationBuilder.RenameIndex(
                name: "IX_OrderEvent_OrderId",
                table: "OrderEvents",
                newName: "IX_OrderEvents_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEvent_GameId",
                table: "GameEvents",
                newName: "IX_GameEvents_GameId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventAt",
                table: "OrderEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 5, 16, 33, 43, 621, DateTimeKind.Local).AddTicks(4021),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 289, DateTimeKind.Local).AddTicks(2196));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventAt",
                table: "GameEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 5, 16, 33, 43, 620, DateTimeKind.Local).AddTicks(2556),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 288, DateTimeKind.Local).AddTicks(1095));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderEvents",
                table: "OrderEvents",
                column: "OrderEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEvents",
                table: "GameEvents",
                column: "GameEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvents_Games_GameId",
                table: "GameEvents",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderEvents_Orders_OrderId",
                table: "OrderEvents",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvents_Games_GameId",
                table: "GameEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderEvents_Orders_OrderId",
                table: "OrderEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderEvents",
                table: "OrderEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEvents",
                table: "GameEvents");

            migrationBuilder.RenameTable(
                name: "OrderEvents",
                newName: "OrderEvent");

            migrationBuilder.RenameTable(
                name: "GameEvents",
                newName: "GameEvent");

            migrationBuilder.RenameIndex(
                name: "IX_OrderEvents_OrderId",
                table: "OrderEvent",
                newName: "IX_OrderEvent_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEvents_GameId",
                table: "GameEvent",
                newName: "IX_GameEvent_GameId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventAt",
                table: "OrderEvent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 289, DateTimeKind.Local).AddTicks(2196),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 10, 5, 16, 33, 43, 621, DateTimeKind.Local).AddTicks(4021));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventAt",
                table: "GameEvent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 5, 16, 32, 44, 288, DateTimeKind.Local).AddTicks(1095),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 10, 5, 16, 33, 43, 620, DateTimeKind.Local).AddTicks(2556));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderEvent",
                table: "OrderEvent",
                column: "OrderEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEvent",
                table: "GameEvent",
                column: "GameEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvent_Games_GameId",
                table: "GameEvent",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderEvent_Orders_OrderId",
                table: "OrderEvent",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
