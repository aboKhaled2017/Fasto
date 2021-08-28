using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class update_Lzdrug_tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseDrugId",
                table: "LzDrugs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugs_BaseDrugId",
                table: "LzDrugs",
                column: "BaseDrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugs_BaseDrugs_BaseDrugId",
                table: "LzDrugs",
                column: "BaseDrugId",
                principalTable: "BaseDrugs",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugs_BaseDrugs_BaseDrugId",
                table: "LzDrugs");

            migrationBuilder.DropIndex(
                name: "IX_LzDrugs_BaseDrugId",
                table: "LzDrugs");

            migrationBuilder.DropColumn(
                name: "BaseDrugId",
                table: "LzDrugs");
        }
    }
}
