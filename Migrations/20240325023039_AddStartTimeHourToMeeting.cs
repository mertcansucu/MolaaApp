using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolaaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStartTimeHourToMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTimeHour",
                table: "meetings",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTimeHour",
                table: "meetings");
        }
    }
}
