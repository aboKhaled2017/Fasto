using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class removePharmaInStkClassTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_Stocks_StockId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropTable(
                name: "PharmaciesInStockClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmaciesInStocks",
                table: "PharmaciesInStocks");

            migrationBuilder.DropIndex(
                name: "IX_PharmaciesInStocks_StockId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "PharmaciesInStocks");

            migrationBuilder.AddColumn<Guid>(
                name: "StockClassId",
                table: "PharmaciesInStocks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PharmacyCustomerId",
                table: "PharmaciesInStocks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmaciesInStocks",
                table: "PharmaciesInStocks",
                columns: new[] { "PharmacyId", "StockClassId" });

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStocks_PharmacyCustomerId",
                table: "PharmaciesInStocks",
                column: "PharmacyCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStocks_StockClassId",
                table: "PharmaciesInStocks",
                column: "StockClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_Pharmacies_PharmacyCustomerId",
                table: "PharmaciesInStocks",
                column: "PharmacyCustomerId",
                principalTable: "Pharmacies",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_StocksWithPharmaClasses_StockClassId",
                table: "PharmaciesInStocks",
                column: "StockClassId",
                principalTable: "StocksWithPharmaClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_Pharmacies_PharmacyCustomerId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_StocksWithPharmaClasses_StockClassId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmaciesInStocks",
                table: "PharmaciesInStocks");

            migrationBuilder.DropIndex(
                name: "IX_PharmaciesInStocks_PharmacyCustomerId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropIndex(
                name: "IX_PharmaciesInStocks_StockClassId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropColumn(
                name: "StockClassId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropColumn(
                name: "PharmacyCustomerId",
                table: "PharmaciesInStocks");

            migrationBuilder.AddColumn<string>(
                name: "StockId",
                table: "PharmaciesInStocks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmaciesInStocks",
                table: "PharmaciesInStocks",
                columns: new[] { "PharmacyId", "StockId" });

            migrationBuilder.CreateTable(
                name: "PharmaciesInStockClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: true),
                    StockClassId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaciesInStockClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_StocksWithPharmaClasses_StockClassId",
                        column: x => x.StockClassId,
                        principalTable: "StocksWithPharmaClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStocks_StockId",
                table: "PharmaciesInStocks",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_StockClassId",
                table: "PharmaciesInStockClasses",
                column: "StockClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_PharmacyId_StockClassId",
                table: "PharmaciesInStockClasses",
                columns: new[] { "PharmacyId", "StockClassId" },
                unique: true,
                filter: "[PharmacyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_Stocks_StockId",
                table: "PharmaciesInStocks",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
