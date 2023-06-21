using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RWSS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventsAndStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearCategory",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearCategory",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_EventStudent_eventsId",
                table: "EventStudent",
                column: "eventsId");
        }
    }
}
