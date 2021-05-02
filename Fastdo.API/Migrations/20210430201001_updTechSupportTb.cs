using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class updTechSupportTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId1",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_BaseCustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_AdminId1",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_BaseCustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "AdminId1",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "BaseCustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "TechnicalSupportQuestions",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_CustomerId",
                table: "TechnicalSupportQuestions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_CustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TechnicalSupportQuestions",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_CustomerId",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "AdminId1",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseCustomerId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_AdminId1",
                table: "TechnicalSupportQuestions",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_BaseCustomerId",
                table: "TechnicalSupportQuestions",
                column: "BaseCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId1",
                table: "TechnicalSupportQuestions",
                column: "AdminId1",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_BaseCustomerId",
                table: "TechnicalSupportQuestions",
                column: "BaseCustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
