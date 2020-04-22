using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202033 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statuss",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "_PaymentsQiwies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_Pd_Pmid",
                table: "PaymentDetails",
                column: "Pd_Pmid");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Payments",
                table: "PaymentDetails",
                column: "Pd_Pmid",
                principalTable: "Payments",
                principalColumn: "Pm_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Payments",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_Pd_Pmid",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "_PaymentsQiwies");

            migrationBuilder.AddColumn<string>(
                name: "Statuss",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
