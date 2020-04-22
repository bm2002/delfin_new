using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _210420209 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "PaymentDetails");

            //migrationBuilder.DropTable(
            //    name: "Payments");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK__PaymentsUniteller",
            //    table: "_PaymentsUniteller");

            //migrationBuilder.RenameTable(
            //    name: "_PaymentsUniteller",
            //    newName: "_PaymentsQiwies");

            //migrationBuilder.RenameColumn(
            //    name: "DG_Code",
            //    table: "_PaymentsQiwies",
            //    newName: "DgCode");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Status",
            //    table: "_PaymentsQiwies",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "varchar(50)",
            //    oldUnicode: false,
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "DateModified",
            //    table: "_PaymentsQiwies",
            //    nullable: true,
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "DateCreate",
            //    table: "_PaymentsQiwies",
            //    nullable: true,
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "Amount",
            //    table: "_PaymentsQiwies",
            //    nullable: true,
            //    oldClrType: typeof(decimal),
            //    oldType: "money",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "DgCode",
            //    table: "_PaymentsQiwies",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "varchar(50)",
            //    oldUnicode: false,
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK__PaymentsQiwies",
            //    table: "_PaymentsQiwies",
            //    column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__PaymentsQiwies",
                table: "_PaymentsQiwies");

            migrationBuilder.RenameTable(
                name: "_PaymentsQiwies",
                newName: "_PaymentsUniteller");

            migrationBuilder.RenameColumn(
                name: "DgCode",
                table: "_PaymentsUniteller",
                newName: "DG_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "_PaymentsUniteller",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "_PaymentsUniteller",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "_PaymentsUniteller",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "_PaymentsUniteller",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DG_Code",
                table: "_PaymentsUniteller",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__PaymentsUniteller",
                table: "_PaymentsUniteller",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PM_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PM_CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PM_CreatorKey = table.Column<int>(type: "int", nullable: false),
                    PM_Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PM_DepartmentKey = table.Column<int>(type: "int", nullable: false),
                    PM_DocumentNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PM_Export = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    PM_FilialKey = table.Column<int>(type: "int", nullable: false),
                    PM_IsDeleted = table.Column<short>(type: "smallint", nullable: true),
                    PM_Number = table.Column<int>(type: "int", nullable: false),
                    PM_ORKey = table.Column<int>(type: "int", nullable: true),
                    PM_POId = table.Column<int>(type: "int", nullable: false),
                    PM_PRKey = table.Column<int>(type: "int", nullable: false),
                    PM_RAKey = table.Column<int>(type: "int", nullable: false),
                    PM_Reason = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: true),
                    PM_RepresentInfo = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    PM_RepresentName = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    PM_Sum = table.Column<decimal>(type: "money", nullable: false),
                    PM_SumNational = table.Column<decimal>(type: "money", nullable: false),
                    PM_Used = table.Column<decimal>(type: "money", nullable: false),
                    PM_VData = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PM_Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    PD_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PD_Course = table.Column<decimal>(type: "decimal(19, 6)", nullable: false),
                    PD_CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PD_CreatorKey = table.Column<int>(type: "int", nullable: false),
                    PD_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    PD_DGKey = table.Column<int>(type: "int", nullable: true),
                    PD_Percent = table.Column<decimal>(type: "money", nullable: false),
                    PD_PMId = table.Column<int>(type: "int", nullable: false),
                    PD_Reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PD_Sum = table.Column<decimal>(type: "money", nullable: false),
                    PD_SumInDogovorRate = table.Column<decimal>(type: "money", nullable: true),
                    PD_SumNational = table.Column<decimal>(type: "money", nullable: false),
                    PD_SumNationalWords = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PD_SumTax1 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    PD_SumTax2 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    PD_SumTaxPercent1 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    PD_SumTaxPercent2 = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.PD_Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Payments",
                        column: x => x.PD_PMId,
                        principalTable: "Payments",
                        principalColumn: "PM_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PD_DGKey",
                table: "PaymentDetails",
                column: "PD_DGKey");

            migrationBuilder.CreateIndex(
                name: "IX_PD_PMId",
                table: "PaymentDetails",
                column: "PD_PMId");

            migrationBuilder.CreateIndex(
                name: "IX_PM_DEL_PO_FI_CR_DATE",
                table: "Payments",
                columns: new[] { "PM_IsDeleted", "PM_POId", "PM_FilialKey", "PM_CreatorKey", "PM_Date" });
        }
    }
}
