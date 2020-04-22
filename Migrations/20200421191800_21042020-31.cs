using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202031 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PaymentDetail_Payment_Pd_Pmid",
            //    table: "PaymentDetail");

            //migrationBuilder.DropIndex(
            //    name: "IX_PaymentDetail_Pd_Pmid",
            //    table: "PaymentDetail");

            //migrationBuilder.RenameTable(
            //    name: "PaymentDetail",
            //    newName: "PaymentDetails");

            //migrationBuilder.RenameTable(
            //    name: "Payment",
            //    newName: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "PaymentDetails",
                newName: "PaymentDetail");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetail_Pd_Pmid",
                table: "PaymentDetail",
                column: "Pd_Pmid");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetail_Payment_Pd_Pmid",
                table: "PaymentDetail",
                column: "Pd_Pmid",
                principalTable: "Payment",
                principalColumn: "Pm_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
