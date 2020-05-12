using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _120520201317 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "paymentIdqiwi",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentIdqiwi",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "paymentId",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
