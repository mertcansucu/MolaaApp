using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolaaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerIdToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizerId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "AspNetUsers");
        }
    }
}
