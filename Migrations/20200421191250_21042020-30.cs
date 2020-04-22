using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202030 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX__PaymentsQiwies_DgCode",
                table: "_PaymentsQiwies");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "constrait",
                table: "_PaymentsQiwies",
                column: "DgCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "constrait",
                table: "_PaymentsQiwies");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX__PaymentsQiwies_DgCode",
                table: "_PaymentsQiwies",
                column: "DgCode",
                unique: true,
                filter: "[DgCode] IS NOT NULL");
        }
    }
}
