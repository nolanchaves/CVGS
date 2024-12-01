using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class AddedFriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRequest",
                columns: table => new
                {
                    FriendRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequest", x => x.FriendRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    FriendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserOneId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserTwoId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.FriendId);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequestUser",
                columns: table => new
                {
                    FriendRequestId = table.Column<int>(type: "int", nullable: false),
                    PrimaryUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequestUser", x => new { x.FriendRequestId, x.PrimaryUserId });
                    table.ForeignKey(
                        name: "FK_FriendRequestUser_AspNetUsers_PrimaryUserId",
                        column: x => x.PrimaryUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequestUser_FriendRequest_FriendRequestId",
                        column: x => x.FriendRequestId,
                        principalTable: "FriendRequest",
                        principalColumn: "FriendRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequestUser1",
                columns: table => new
                {
                    FriendRequest1FriendRequestId = table.Column<int>(type: "int", nullable: false),
                    SecondaryUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequestUser1", x => new { x.FriendRequest1FriendRequestId, x.SecondaryUserId });
                    table.ForeignKey(
                        name: "FK_FriendRequestUser1_AspNetUsers_SecondaryUserId",
                        column: x => x.SecondaryUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequestUser1_FriendRequest_FriendRequest1FriendRequestId",
                        column: x => x.FriendRequest1FriendRequestId,
                        principalTable: "FriendRequest",
                        principalColumn: "FriendRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendsUser",
                columns: table => new
                {
                    FriendsFriendId = table.Column<int>(type: "int", nullable: false),
                    UserOneId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsUser", x => new { x.FriendsFriendId, x.UserOneId });
                    table.ForeignKey(
                        name: "FK_FriendsUser_AspNetUsers_UserOneId",
                        column: x => x.UserOneId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendsUser_Friends_FriendsFriendId",
                        column: x => x.FriendsFriendId,
                        principalTable: "Friends",
                        principalColumn: "FriendId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendsUser1",
                columns: table => new
                {
                    Friends1FriendId = table.Column<int>(type: "int", nullable: false),
                    UserTwoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsUser1", x => new { x.Friends1FriendId, x.UserTwoId });
                    table.ForeignKey(
                        name: "FK_FriendsUser1_AspNetUsers_UserTwoId",
                        column: x => x.UserTwoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendsUser1_Friends_Friends1FriendId",
                        column: x => x.Friends1FriendId,
                        principalTable: "Friends",
                        principalColumn: "FriendId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FriendRequest",
                columns: new[] { "FriendRequestId", "PrimaryUserId", "SecondaryUserId", "Status" },
                values: new object[] { 1, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2", "042f95a1-3247-4e6a-a375-d2165a8bf16c", "Pending" });

            migrationBuilder.InsertData(
                table: "Friends",
                columns: new[] { "FriendId", "UserOneId", "UserTwoId" },
                values: new object[,]
                {
                    { 1, "042f95a1-3247-4e6a-a375-d2165a8bf16c", "e091dbd6-3f3f-4177-87f1-c8a7b2674a9f" },
                    { 2, "042f95a1-3247-4e6a-a375-d2165a8bf16c", "88193658-5295-478b-9f7e-534f739a06bc" },
                    { 3, "042f95a1-3247-4e6a-a375-d2165a8bf16c", "05e61254-11dc-44d9-89e7-0e574ce7099a" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequestUser_PrimaryUserId",
                table: "FriendRequestUser",
                column: "PrimaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequestUser1_SecondaryUserId",
                table: "FriendRequestUser1",
                column: "SecondaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsUser_UserOneId",
                table: "FriendsUser",
                column: "UserOneId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsUser1_UserTwoId",
                table: "FriendsUser1",
                column: "UserTwoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequestUser");

            migrationBuilder.DropTable(
                name: "FriendRequestUser1");

            migrationBuilder.DropTable(
                name: "FriendsUser");

            migrationBuilder.DropTable(
                name: "FriendsUser1");

            migrationBuilder.DropTable(
                name: "FriendRequest");

            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}
