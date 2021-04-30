using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class editTEchSupportTbSchem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "TechnicalSupportQuestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PharmacyId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_AdminId",
                table: "TechnicalSupportQuestions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_PharmacyId",
                table: "TechnicalSupportQuestions",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_StockId",
                table: "TechnicalSupportQuestions",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId",
                table: "TechnicalSupportQuestions",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_PharmacyId",
                table: "TechnicalSupportQuestions",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_StockId",
                table: "TechnicalSupportQuestions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_PharmacyId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_StockId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_AdminId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_PharmacyId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_StockId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "TechnicalSupportQuestions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
