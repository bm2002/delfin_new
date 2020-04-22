using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202039 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentKey1",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<int>(
                name: "PaymentKey",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentKey",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<int>(
                name: "PaymentKey1",
                table: "_PaymentsQiwies",
                type: "int",
                nullable: true);
        }
    }
}
