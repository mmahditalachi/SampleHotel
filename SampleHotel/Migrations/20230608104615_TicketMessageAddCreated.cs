using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleHotel.Migrations
{
    /// <inheritdoc />
    public partial class TicketMessageAddCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketMessage_AspNetUsers_Receiver",
                table: "TicketMessage");

            migrationBuilder.DropIndex(
                name: "IX_TicketMessage_Receiver",
                table: "TicketMessage");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "TicketMessage");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TicketMessage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "TicketMessage");

            migrationBuilder.AddColumn<Guid>(
                name: "Receiver",
                table: "TicketMessage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessage_Receiver",
                table: "TicketMessage",
                column: "Receiver");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMessage_AspNetUsers_Receiver",
                table: "TicketMessage",
                column: "Receiver",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
