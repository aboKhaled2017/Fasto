using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class updateTechSupportEntity_removeResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Response",
                table: "TechnicalSupportQuestions");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "TechnicalSupportQuestions",
                newName: "Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "TechnicalSupportQuestions",
                newName: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "TechnicalSupportQuestions",
                nullable: true);
        }
    }
}
