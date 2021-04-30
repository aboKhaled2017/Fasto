using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class updateTechSupportEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RelatedToId",
                table: "TechnicalSupportQuestions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_RelatedToId",
                table: "TechnicalSupportQuestions",
                column: "RelatedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_TechnicalSupportQuestions_RelatedToId",
                table: "TechnicalSupportQuestions",
                column: "RelatedToId",
                principalTable: "TechnicalSupportQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_TechnicalSupportQuestions_RelatedToId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_RelatedToId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropColumn(
                name: "RelatedToId",
                table: "TechnicalSupportQuestions");
        }
    }
}
