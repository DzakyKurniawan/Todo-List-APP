using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Client.Migrations
{
    public partial class AddRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessTokenExpiration",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Expiration = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccessTokenExpiration",
                table: "AspNetUsers");
        }
    }
}
