using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "Payments",
            //    newName: "Payment");

            //migrationBuilder.RenameTable(
            //    name: "PaymentDetails",
            //    newName: "PaymentDetail");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX__PaymentsQiwies_DgCode",
                table: "_PaymentsQiwies",
                column: "DgCode",
                unique: true,
                filter: "[DgCode] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentDetail_Pd_Pmid",
            //    table: "PaymentDetail",
            //    column: "Pd_Pmid");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PaymentDetail_Payment_Pd_Pmid",
            //    table: "PaymentDetail",
            //    column: "Pd_Pmid",
            //    principalTable: "Payment",
            //    principalColumn: "Pm_Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetail_Payment_Pd_Pmid",
                table: "PaymentDetail");

            migrationBuilder.DropIndex(
                name: "IX__PaymentsQiwies_DgCode",
                table: "_PaymentsQiwies");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetail_Pd_Pmid",
                table: "PaymentDetail");

            migrationBuilder.RenameTable(
                name: "PaymentDetail",
                newName: "PaymentDetails");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
