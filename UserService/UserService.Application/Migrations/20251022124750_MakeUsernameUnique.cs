using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Application.Migrations
{
    /// <inheritdoc />
    public partial class MakeUsernameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Username",
                table: "User");
        }
    }
}
