using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class editTEchSupportTbSchem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions",
                column: "TechnicalSupportQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_TechnicalSupportQuestions_TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions",
                column: "TechnicalSupportQuestionId",
                principalTable: "TechnicalSupportQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_TechnicalSupportQuestions_TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "TechnicalSupportQuestionId",
                table: "TechnicalSupportQuestions");
        }
    }
}
