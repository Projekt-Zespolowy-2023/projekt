using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RWSS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToSingleTableForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_StudentId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_DeaneryWorkers_DeaneryWorkerId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "DeaneryWorkers");

            migrationBuilder.DropIndex(
                name: "IX_Events_DeaneryWorkerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DeaneryWorkerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DegreeCourse",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IndexNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserCategory",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Events",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_StudentId",
                table: "Events",
                newName: "IX_Events_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_AppUserId",
                table: "Events",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AppUserId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Events",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_AppUserId",
                table: "Events",
                newName: "IX_Events_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "DeaneryWorkerId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DegreeCourse",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IndexNumber",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserCategory",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeaneryWorkers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserCategory = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeaneryWorkers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_DeaneryWorkerId",
                table: "Events",
                column: "DeaneryWorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_StudentId",
                table: "Events",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_DeaneryWorkers_DeaneryWorkerId",
                table: "Events",
                column: "DeaneryWorkerId",
                principalTable: "DeaneryWorkers",
                principalColumn: "Id");
        }
    }
}
