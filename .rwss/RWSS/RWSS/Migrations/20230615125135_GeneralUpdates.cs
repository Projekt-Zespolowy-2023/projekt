using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RWSS.Migrations
{
    /// <inheritdoc />
    public partial class GeneralUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeaneryWorkers_AspNetUsers_AppUserId",
                table: "DeaneryWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudiesDegree",
                table: "Students",
                newName: "StudiesDegreeCategory");

            migrationBuilder.RenameColumn(
                name: "DegreeCourse",
                table: "Students",
                newName: "DegreeCourseCategory");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "DeaneryWorkers",
                newName: "PositionCategory");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "DeaneryWorkers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_DeaneryWorkers_AspNetUsers_AppUserId",
                table: "DeaneryWorkers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeaneryWorkers_AspNetUsers_AppUserId",
                table: "DeaneryWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudiesDegreeCategory",
                table: "Students",
                newName: "StudiesDegree");

            migrationBuilder.RenameColumn(
                name: "DegreeCourseCategory",
                table: "Students",
                newName: "DegreeCourse");

            migrationBuilder.RenameColumn(
                name: "PositionCategory",
                table: "DeaneryWorkers",
                newName: "Position");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "DeaneryWorkers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeaneryWorkers_AspNetUsers_AppUserId",
                table: "DeaneryWorkers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
