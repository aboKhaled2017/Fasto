using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class update_Lzdrug_remane_code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugs_BaseDrugs_BaseDrugId",
                table: "LzDrugs");

            migrationBuilder.RenameColumn(
                name: "BaseDrugId",
                table: "LzDrugs",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_LzDrugs_BaseDrugId",
                table: "LzDrugs",
                newName: "IX_LzDrugs_Code");

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugs_BaseDrugs_Code",
                table: "LzDrugs",
                column: "Code",
                principalTable: "BaseDrugs",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugs_BaseDrugs_Code",
                table: "LzDrugs");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "LzDrugs",
                newName: "BaseDrugId");

            migrationBuilder.RenameIndex(
                name: "IX_LzDrugs_Code",
                table: "LzDrugs",
                newName: "IX_LzDrugs_BaseDrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugs_BaseDrugs_BaseDrugId",
                table: "LzDrugs",
                column: "BaseDrugId",
                principalTable: "BaseDrugs",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
