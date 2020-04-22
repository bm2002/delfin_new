using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202034 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Statusss",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statusss",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
