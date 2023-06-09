using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleHotel.Migrations
{
    /// <inheritdoc />
    public partial class RoomReserveAddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RoomReserve",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RoomReserve_UserId",
                table: "RoomReserve",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReserve_AspNetUsers_UserId",
                table: "RoomReserve",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomReserve_AspNetUsers_UserId",
                table: "RoomReserve");

            migrationBuilder.DropIndex(
                name: "IX_RoomReserve_UserId",
                table: "RoomReserve");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RoomReserve");
        }
    }
}
