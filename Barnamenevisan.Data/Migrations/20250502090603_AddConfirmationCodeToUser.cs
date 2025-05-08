using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barnamenevisan.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationCodeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Users");
        }
    }
}
