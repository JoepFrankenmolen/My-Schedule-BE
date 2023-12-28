using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Schedule.UserService.Migrations
{
    /// <inheritdoc />
    public partial class v14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthDetails");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "LastLoginTimestamp",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserSecurities",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurities", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserSecurities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginTimestamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserAuthDetails",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LastLoginTimestamp = table.Column<long>(type: "bigint", nullable: false),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthDetails", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserAuthDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }
    }
}