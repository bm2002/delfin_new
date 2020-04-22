using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Pd_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pd_CreateDate = table.Column<DateTime>(nullable: false),
                    Pd_CreatorKey = table.Column<int>(nullable: false),
                    Pd_Date = table.Column<DateTime>(nullable: false),
                    Pd_Course = table.Column<decimal>(nullable: false),
                    Pd_Percent = table.Column<decimal>(nullable: false),
                    Pd_Sum = table.Column<decimal>(nullable: false),
                    Pd_SumNational = table.Column<decimal>(nullable: false),
                    Pd_SumInDogovorRate = table.Column<decimal>(nullable: true),
                    Pd_SumTax1 = table.Column<decimal>(nullable: true),
                    Pd_SumTaxPercent1 = table.Column<decimal>(nullable: true),
                    Pd_SumTax2 = table.Column<decimal>(nullable: true),
                    Pd_SumTaxPercent2 = table.Column<decimal>(nullable: true),
                    Pd_SumNationalWords = table.Column<string>(nullable: true),
                    Pd_Reason = table.Column<string>(nullable: true),
                    Pd_Dgkey = table.Column<int>(nullable: true),
                    Pd_Pmid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_PaymentDetail", x => x.Pd_Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Pm_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pm_CreateDate = table.Column<DateTime>(nullable: false),
                    Pm_CreatorKey = table.Column<int>(nullable: false),
                    Pm_FilialKey = table.Column<int>(nullable: false),
                    Pm_DepartmentKey = table.Column<int>(nullable: false),
                    Pm_DocumentNumber = table.Column<string>(nullable: true),
                    Pm_Number = table.Column<int>(nullable: false),
                    Pm_Prkey = table.Column<int>(nullable: false),
                    Pm_Poid = table.Column<int>(nullable: false),
                    Pm_Sum = table.Column<decimal>(nullable: false),
                    Pm_Rakey = table.Column<int>(nullable: false),
                    Pm_SumNational = table.Column<decimal>(nullable: false),
                    Pm_Used = table.Column<decimal>(nullable: false),
                    Pm_RepresentName = table.Column<string>(nullable: true),
                    Pm_RepresentInfo = table.Column<string>(nullable: true),
                    Pm_Reason = table.Column<string>(nullable: true),
                    Pm_Export = table.Column<int>(nullable: false),
                    Pm_Orkey = table.Column<int>(nullable: true),
                    Pm_IsDeleted = table.Column<short>(nullable: true),
                    Pm_Date = table.Column<DateTime>(nullable: false),
                    Pm_Vdata = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Payment", x => x.Pm_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
