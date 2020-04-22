using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "_PaymentsUniteller",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DG_Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        Amount = table.Column<decimal>(type: "money", nullable: true),
            //        DateCreate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Status = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        PaymentKey = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__PaymentsUniteller", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Payments",
            //    columns: table => new
            //    {
            //        PM_Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PM_CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        PM_CreatorKey = table.Column<int>(nullable: false),
            //        PM_FilialKey = table.Column<int>(nullable: false),
            //        PM_DepartmentKey = table.Column<int>(nullable: false),
            //        PM_DocumentNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
            //        PM_Number = table.Column<int>(nullable: false),
            //        PM_PRKey = table.Column<int>(nullable: false),
            //        PM_POId = table.Column<int>(nullable: false),
            //        PM_Sum = table.Column<decimal>(type: "money", nullable: false),
            //        PM_RAKey = table.Column<int>(nullable: false),
            //        PM_SumNational = table.Column<decimal>(type: "money", nullable: false),
            //        PM_Used = table.Column<decimal>(type: "money", nullable: false),
            //        PM_RepresentName = table.Column<string>(unicode: false, maxLength: 120, nullable: true),
            //        PM_RepresentInfo = table.Column<string>(unicode: false, maxLength: 120, nullable: true),
            //        PM_Reason = table.Column<string>(unicode: false, maxLength: 4000, nullable: true),
            //        PM_Export = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
            //        PM_ORKey = table.Column<int>(nullable: true),
            //        PM_IsDeleted = table.Column<short>(nullable: true),
            //        PM_Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        PM_VData = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Payments", x => x.PM_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PaymentDetails",
            //    columns: table => new
            //    {
            //        PD_Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PD_CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        PD_CreatorKey = table.Column<int>(nullable: false),
            //        PD_Date = table.Column<DateTime>(type: "datetime", nullable: false),
            //        PD_Course = table.Column<decimal>(type: "decimal(19, 6)", nullable: false),
            //        PD_Percent = table.Column<decimal>(type: "money", nullable: false),
            //        PD_Sum = table.Column<decimal>(type: "money", nullable: false),
            //        PD_SumNational = table.Column<decimal>(type: "money", nullable: false),
            //        PD_SumInDogovorRate = table.Column<decimal>(type: "money", nullable: true),
            //        PD_SumTax1 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
            //        PD_SumTaxPercent1 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
            //        PD_SumTax2 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
            //        PD_SumTaxPercent2 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((1))"),
            //        PD_SumNationalWords = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
            //        PD_Reason = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
            //        PD_DGKey = table.Column<int>(nullable: true),
            //        PD_PMId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PaymentDetails", x => x.PD_Id);
            //        table.ForeignKey(
            //            name: "FK_PaymentDetails_Payments",
            //            column: x => x.PD_PMId,
            //            principalTable: "Payments",
            //            principalColumn: "PM_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PD_DGKey",
            //    table: "PaymentDetails",
            //    column: "PD_DGKey");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PD_PMId",
            //    table: "PaymentDetails",
            //    column: "PD_PMId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PM_DEL_PO_FI_CR_DATE",
            //    table: "Payments",
            //    columns: new[] { "PM_IsDeleted", "PM_POId", "PM_FilialKey", "PM_CreatorKey", "PM_Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_PaymentsUniteller");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
