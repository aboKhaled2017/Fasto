using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class editTechSupportTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_AspNetUsers_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_AspNetUsers_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
