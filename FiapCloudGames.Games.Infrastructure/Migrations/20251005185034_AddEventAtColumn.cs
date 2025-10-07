using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventAt",
                table: "OrderUpdate",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 5, 15, 50, 33, 957, DateTimeKind.Local).AddTicks(6217));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventAt",
                table: "OrderUpdate");
        }
    }
}
