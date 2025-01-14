using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keycontrol.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class keydescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_keys_rooms_RoomId",
                table: "keys");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "IdKey",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "WithdrawalDate",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "IdRoom",
                table: "keys");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "keys",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_keys_rooms_RoomId",
                table: "keys",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_keys_rooms_RoomId",
                table: "keys");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "rooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdKey",
                table: "reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithdrawalDate",
                table: "reports",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "keys",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdRoom",
                table: "keys",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_keys_rooms_RoomId",
                table: "keys",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
