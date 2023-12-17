using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerSite2.Data.Migrations
{
    /// <inheritdoc />
    public partial class SS23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetSearchId",
                table: "UserBets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetSearchId",
                table: "UserBets");
        }
    }
}
