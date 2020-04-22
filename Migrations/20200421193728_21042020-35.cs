using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202035 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statusss",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Statusss1",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statusss1",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Statusss",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
