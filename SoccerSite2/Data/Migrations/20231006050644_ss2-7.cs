using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerSite2.Data.Migrations
{
    /// <inheritdoc />
    public partial class ss27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamChoice",
                table: "UserBets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamChoice",
                table: "UserBets");
        }
    }
}
