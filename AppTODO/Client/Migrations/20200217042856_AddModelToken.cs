using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Client.Migrations
{
    public partial class AddModelToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    ExpireToken = table.Column<long>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    ExpireRefreshToken = table.Column<long>(nullable: false),
                    UsernameId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_AspNetUsers_UsernameId",
                        column: x => x.UsernameId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Token_UsernameId",
                table: "Token",
                column: "UsernameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });
        }
    }
}
