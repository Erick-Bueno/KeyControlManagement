using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keycontrol.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeiv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iv",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iv",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
