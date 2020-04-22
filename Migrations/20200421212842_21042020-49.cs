using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202049 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentKey2",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<int>(
                name: "PaymentKey3",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentKey3",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<int>(
                name: "PaymentKey2",
                table: "_PaymentsQiwies",
                type: "int",
                nullable: true);
        }
    }
}
