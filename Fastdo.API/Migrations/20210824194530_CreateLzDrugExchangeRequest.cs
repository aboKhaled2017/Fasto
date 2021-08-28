using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class CreateLzDrugExchangeRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LzDrugExchangeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    LzDrugId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LzDrugExchangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LzDrugExchangeRequests_LzDrugs_LzDrugId",
                        column: x => x.LzDrugId,
                        principalTable: "LzDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LzDrugExchangeRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugExchangeRequests_LzDrugId",
                table: "LzDrugExchangeRequests",
                column: "LzDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugExchangeRequests_PharmacyId",
                table: "LzDrugExchangeRequests",
                column: "PharmacyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LzDrugExchangeRequests");
        }
    }
}
