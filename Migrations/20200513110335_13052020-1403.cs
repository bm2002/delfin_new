using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _130520201403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "paymentId",
                table: "_PaymentsQiwies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "_PaymentsQiwies");
        }
    }
}
