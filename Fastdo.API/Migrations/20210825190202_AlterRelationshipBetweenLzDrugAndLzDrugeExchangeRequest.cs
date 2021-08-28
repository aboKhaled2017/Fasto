using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class AlterRelationshipBetweenLzDrugAndLzDrugeExchangeRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugExchangeRequests_LzDrugs_LzDrugId",
                table: "LzDrugExchangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_LzDrugExchangeRequests_LzDrugId",
                table: "LzDrugExchangeRequests");

            migrationBuilder.DropColumn(
                name: "LzDrugId",
                table: "LzDrugExchangeRequests");

            migrationBuilder.CreateTable(
                name: "LzDrugLzDrugExchangeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LzDrugId = table.Column<Guid>(nullable: false),
                    LzDrugExchangeRequestId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LzDrugLzDrugExchangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LzDrugLzDrugExchangeRequest_LzDrugExchangeRequests_LzDrugExchangeRequestId",
                        column: x => x.LzDrugExchangeRequestId,
                        principalTable: "LzDrugExchangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LzDrugLzDrugExchangeRequest_LzDrugs_LzDrugId",
                        column: x => x.LzDrugId,
                        principalTable: "LzDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugLzDrugExchangeRequest_LzDrugExchangeRequestId",
                table: "LzDrugLzDrugExchangeRequest",
                column: "LzDrugExchangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugLzDrugExchangeRequest_LzDrugId",
                table: "LzDrugLzDrugExchangeRequest",
                column: "LzDrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LzDrugLzDrugExchangeRequest");

            migrationBuilder.AddColumn<Guid>(
                name: "LzDrugId",
                table: "LzDrugExchangeRequests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugExchangeRequests_LzDrugId",
                table: "LzDrugExchangeRequests",
                column: "LzDrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugExchangeRequests_LzDrugs_LzDrugId",
                table: "LzDrugExchangeRequests",
                column: "LzDrugId",
                principalTable: "LzDrugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
