using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Schedule.UserService.Migrations
{
    /// <inheritdoc />
    public partial class v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthDetails",
                table: "UserAuthDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthDetails_UserId",
                table: "UserAuthDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAuthDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthDetails",
                table: "UserAuthDetails",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthDetails",
                table: "UserAuthDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserAuthDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthDetails",
                table: "UserAuthDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthDetails_UserId",
                table: "UserAuthDetails",
                column: "UserId");
        }
    }
}
