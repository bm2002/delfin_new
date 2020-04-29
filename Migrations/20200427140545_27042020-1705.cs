using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _270420201705 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentKey",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "billId",
                table: "_PaymentsQiwies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payUrl",
                table: "_PaymentsQiwies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "response",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billId",
                table: "_PaymentsQiwies");

            migrationBuilder.DropColumn(
                name: "payUrl",
                table: "_PaymentsQiwies");

            migrationBuilder.DropColumn(
                name: "response",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<int>(
                name: "PaymentKey",
                table: "_PaymentsQiwies",
                type: "int",
                nullable: true);
        }
    }
}
