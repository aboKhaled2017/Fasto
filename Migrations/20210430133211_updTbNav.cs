using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class updTbNav : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugRequests_Pharmacies_PharmacyId",
                table: "LzDrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugs_Pharmacies_PharmacyId",
                table: "LzDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_Pharmacies_PharmacyId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_Stocks_StockId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugPackagesRequests_Pharmacies_PharmacyId",
                table: "StkDrugPackagesRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugs_Stocks_StockId",
                table: "StkDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Areas_AreaId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_Id",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Pharmacies_PharmacyId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Stocks_StockId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.RenameTable(
                name: "Stocks",
                newName: "BaseCustomer");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "TechnicalSupportQuestions",
                newName: "BaseCustomerId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "TechnicalSupportQuestions",
                newName: "AdminId1");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_StockId",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_BaseCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_PharmacyId",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_AdminId1");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_AreaId",
                table: "BaseCustomer",
                newName: "IX_BaseCustomer_AreaId");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "TechnicalSupportQuestions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "BaseCustomer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                table: "BaseCustomer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MgrName",
                table: "BaseCustomer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LicenseImgSrc",
                table: "BaseCustomer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CommercialRegImgSrc",
                table: "BaseCustomer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseCustomer",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stock_CommercialRegImgSrc",
                table: "BaseCustomer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stock_LicenseImgSrc",
                table: "BaseCustomer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stock_MgrName",
                table: "BaseCustomer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stock_OwnerName",
                table: "BaseCustomer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock_Status",
                table: "BaseCustomer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseCustomer",
                table: "BaseCustomer",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseCustomer_Areas_AreaId",
                table: "BaseCustomer",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseCustomer_AspNetUsers_Id",
                table: "BaseCustomer",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugRequests_BaseCustomer_PharmacyId",
                table: "LzDrugRequests",
                column: "PharmacyId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugs_BaseCustomer_PharmacyId",
                table: "LzDrugs",
                column: "PharmacyId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStockClasses_BaseCustomer_PharmacyId",
                table: "PharmaciesInStockClasses",
                column: "PharmacyId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_BaseCustomer_PharmacyId",
                table: "PharmaciesInStocks",
                column: "PharmacyId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_BaseCustomer_StockId",
                table: "PharmaciesInStocks",
                column: "StockId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugPackagesRequests_BaseCustomer_PharmacyId",
                table: "StkDrugPackagesRequests",
                column: "PharmacyId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugs_BaseCustomer_StockId",
                table: "StkDrugs",
                column: "StockId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StocksWithPharmaClasses_BaseCustomer_StockId",
                table: "StocksWithPharmaClasses",
                column: "StockId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId1",
                table: "TechnicalSupportQuestions",
                column: "AdminId1",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_BaseCustomerId",
                table: "TechnicalSupportQuestions",
                column: "BaseCustomerId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseCustomer_Areas_AreaId",
                table: "BaseCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseCustomer_AspNetUsers_Id",
                table: "BaseCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugRequests_BaseCustomer_PharmacyId",
                table: "LzDrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LzDrugs_BaseCustomer_PharmacyId",
                table: "LzDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStockClasses_BaseCustomer_PharmacyId",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_BaseCustomer_PharmacyId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStocks_BaseCustomer_StockId",
                table: "PharmaciesInStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugPackagesRequests_BaseCustomer_PharmacyId",
                table: "StkDrugPackagesRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugs_BaseCustomer_StockId",
                table: "StkDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_StocksWithPharmaClasses_BaseCustomer_StockId",
                table: "StocksWithPharmaClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Admins_AdminId1",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_BaseCustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseCustomer",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Stock_CommercialRegImgSrc",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Stock_LicenseImgSrc",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Stock_MgrName",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Stock_OwnerName",
                table: "BaseCustomer");

            migrationBuilder.DropColumn(
                name: "Stock_Status",
                table: "BaseCustomer");

            migrationBuilder.RenameTable(
                name: "BaseCustomer",
                newName: "Stocks");

            migrationBuilder.RenameColumn(
                name: "BaseCustomerId",
                table: "TechnicalSupportQuestions",
                newName: "StockId");

            migrationBuilder.RenameColumn(
                name: "AdminId1",
                table: "TechnicalSupportQuestions",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_BaseCustomerId",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_StockId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalSupportQuestions_AdminId1",
                table: "TechnicalSupportQuestions",
                newName: "IX_TechnicalSupportQuestions_PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_BaseCustomer_AreaId",
                table: "Stocks",
                newName: "IX_Stocks_AreaId");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "TechnicalSupportQuestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MgrName",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenseImgSrc",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommercialRegImgSrc",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    AreaId = table.Column<byte>(nullable: false),
                    CommercialRegImgSrc = table.Column<string>(nullable: false),
                    LandlinePhone = table.Column<string>(nullable: false),
                    LicenseImgSrc = table.Column<string>(nullable: false),
                    MgrName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OwnerName = table.Column<string>(nullable: false),
                    PersPhone = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pharmacies_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_AreaId",
                table: "Pharmacies",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugRequests_Pharmacies_PharmacyId",
                table: "LzDrugRequests",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LzDrugs_Pharmacies_PharmacyId",
                table: "LzDrugs",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_Pharmacies_PharmacyId",
                table: "PharmaciesInStocks",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStocks_Stocks_StockId",
                table: "PharmaciesInStocks",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugPackagesRequests_Pharmacies_PharmacyId",
                table: "StkDrugPackagesRequests",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugs_Stocks_StockId",
                table: "StkDrugs",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Areas_AreaId",
                table: "Stocks",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_Id",
                table: "Stocks",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
