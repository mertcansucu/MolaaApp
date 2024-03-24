using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolaaApp.Migrations
{
    /// <inheritdoc />
    public partial class TabloDegisiklikleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_meetings_AspNetUsers_OrganizerId",
                table: "meetings");

            migrationBuilder.DropColumn(
                name: "IsAttending",
                table: "userMeetings");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "meetings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_meetings_OrganizerId",
                table: "meetings",
                newName: "IX_meetings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_meetings_AspNetUsers_UserId",
                table: "meetings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_meetings_AspNetUsers_UserId",
                table: "meetings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "meetings",
                newName: "OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_meetings_UserId",
                table: "meetings",
                newName: "IX_meetings_OrganizerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAttending",
                table: "userMeetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_meetings_AspNetUsers_OrganizerId",
                table: "meetings",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
