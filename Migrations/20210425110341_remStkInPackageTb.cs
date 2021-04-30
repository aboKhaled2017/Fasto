﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class remStkInPackageTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugInStkDrgPackagesRequests_StockInStkDrgPackageReqs_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.DropTable(
                name: "StockInStkDrgPackageReqs");

            migrationBuilder.DropColumn(
                name: "PackageDetails",
                table: "StkDrugPackagesRequests");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.RenameColumn(
                name: "StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "StkDrugPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkDrugPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "IX_StkDrugInStkDrgPackagesRequests_StkDrugPackageId");

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "StkDrugInStkDrgPackagesRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StkDrugInStkDrgPackagesRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugPackagesRequests_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                column: "StkDrugPackageId",
                principalTable: "StkDrugPackagesRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugPackagesRequests_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.DropColumn(
                name: "Seen",
                table: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.RenameColumn(
                name: "StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "StkPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                newName: "IX_StkDrugInStkDrgPackagesRequests_StkPackageId");

            migrationBuilder.AddColumn<string>(
                name: "PackageDetails",
                table: "StkDrugPackagesRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StockId",
                table: "StkDrugInStkDrgPackagesRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StockInStkDrgPackageReqs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PackageId = table.Column<Guid>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StockId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInStkDrgPackageReqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockInStkDrgPackageReqs_StkDrugPackagesRequests_PackageId",
                        column: x => x.PackageId,
                        principalTable: "StkDrugPackagesRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockInStkDrgPackageReqs_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockInStkDrgPackageReqs_PackageId",
                table: "StockInStkDrgPackageReqs",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInStkDrgPackageReqs_StockId_PackageId",
                table: "StockInStkDrgPackageReqs",
                columns: new[] { "StockId", "PackageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugInStkDrgPackagesRequests_StockInStkDrgPackageReqs_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                column: "StkPackageId",
                principalTable: "StockInStkDrgPackageReqs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
