using Microsoft.EntityFrameworkCore.Migrations;

namespace qiwi.Migrations
{
    public partial class _2104202032 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "constrait",
                table: "_PaymentsQiwies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "_PaymentsQiwies");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Statuss",
                table: "_PaymentsQiwies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statuss",
                table: "_PaymentsQiwies");

            migrationBuilder.AlterColumn<string>(
                name: "DgCode",
                table: "_PaymentsQiwies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "_PaymentsQiwies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "constrait",
                table: "_PaymentsQiwies",
                column: "DgCode");
        }
    }
}
