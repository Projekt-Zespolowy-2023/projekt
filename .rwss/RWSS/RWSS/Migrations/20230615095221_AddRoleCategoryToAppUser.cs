using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RWSS.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleCategoryToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Students",
                newName: "YearCategory");

            migrationBuilder.RenameColumn(
                name: "Semester",
                table: "Students",
                newName: "SemesterCategory");

            migrationBuilder.AddColumn<int>(
                name: "RoleCategory",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleCategory",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "YearCategory",
                table: "Students",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "SemesterCategory",
                table: "Students",
                newName: "Semester");
        }
    }
}
